using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR.Client;
using WeBoard.Core.Network.Dtos;


public class BoardHubClient : IDisposable
{
    public event Action<Update>? OnUpdateReceived;
    public event Action<List<Update>>? OnInitialSync;
    public event Action? OnAuthFailed;
    public event Action? OnAccessDenied;
    public event Action? OnConnectionClosed;

    private HubConnection? _hubConnection;
    private readonly string _hubUrl;
    private readonly string _authToken;
    private readonly Guid _boardId;
    private long _lastUpdateId;
    private bool _isConnected;
    private readonly ConcurrentQueue<Update> _outgoingQueue = new();

    public BoardHubClient(string hubUrl, string authToken, Guid boardId, long lastUpdateId)
    {
        _hubUrl = hubUrl;
        _authToken = authToken;
        _boardId = boardId;
        _lastUpdateId = lastUpdateId;
    }

    public async Task ConnectAsync()
    {
        if (_isConnected) return;

        _hubConnection = new HubConnectionBuilder()
            .WithUrl(_hubUrl, options =>
            {
                options.AccessTokenProvider = () => Task.FromResult(_authToken)!;
            })
            .WithAutomaticReconnect(new[] { TimeSpan.Zero, TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(5) })
            .Build();

        // Setup handlers
        _hubConnection.On<Update>("ReceiveUpdate", HandleUpdate);
        _hubConnection.On<List<Update>>("InitialSync", HandleInitialSync);
        _hubConnection.On("AuthFailed", () => OnAuthFailed?.Invoke());
        _hubConnection.On("AccessDenied", () => OnAccessDenied?.Invoke());
        _hubConnection.Closed += HandleConnectionClosed;

        try
        {
            await _hubConnection.StartAsync();
            _isConnected = true;

            // Join board after connection
            await _hubConnection.InvokeAsync("JoinBoard", _boardId, _authToken, _lastUpdateId);

            // Start processing queue
            _ = ProcessOutgoingQueue();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Connection failed: {ex.Message}");
        }
    }

    public void QueueUpdate(Update update)
    {
        _outgoingQueue.Enqueue(update);
    }

    private async Task ProcessOutgoingQueue()
    {
        while (_isConnected)
        {
            if (_outgoingQueue.TryDequeue(out var update))
            {
                try
                {
                    await _hubConnection!.InvokeAsync("SendUpdate", _boardId, update);

                    // Update last processed ID
                    if (update.Id > _lastUpdateId)
                        _lastUpdateId = update.Id;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to send update: {ex.Message}");
                }
            }
            await Task.Delay(10);
        }
    }

    private void HandleUpdate(Update update)
    {
        if (update.Id <= _lastUpdateId) return;

        _lastUpdateId = update.Id;
        OnUpdateReceived?.Invoke(update);
    }

    private void HandleInitialSync(List<Update> updates)
    {
        if (updates.Count > 0)
        {
            _lastUpdateId = updates.Max(u => u.Id);
        }
        OnInitialSync?.Invoke(updates);
    }

    private Task HandleConnectionClosed(Exception? exception)
    {
        _isConnected = false;
        OnConnectionClosed?.Invoke();
        return Task.CompletedTask;
    }

    public async Task DisconnectAsync()
    {
        if (_hubConnection != null)
        {
            await _hubConnection.StopAsync();
            await _hubConnection.DisposeAsync();
        }
        _isConnected = false;
    }

    public void Dispose() => DisconnectAsync().Wait();
}