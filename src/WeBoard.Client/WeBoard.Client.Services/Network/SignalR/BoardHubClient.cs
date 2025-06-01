using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR.Client;
using WeBoard.Core.Network.Dtos;

public class BoardHubClient : IDisposable
{
    public event Action<NetworkUpdate>? OnUpdateReceived;
    public event Action<List<NetworkUpdate>>? OnInitialSync;
    public event Action? OnAuthFailed;
    public event Action? OnAccessDenied;
    public event Action? OnConnectionClosed;
    public event Action<BoardMate>? OnBoardMateJoin;
    public event Action<BoardMate>? OnReceiveUserInfo;
    public event Action<string>? OnRemoteCursorUpdate;

    private HubConnection? _hubConnection;
    private readonly string _hubUrl;
    private readonly string _authToken;
    private readonly Guid _boardId;
    private long _lastUpdateId;
    private bool _isConnected;
    private readonly ConcurrentQueue<NetworkUpdate> _outgoingQueue = new();
    private readonly ConcurrentQueue<string> _outgoingCursorUpdatesQueue= new();

    public ConcurrentDictionary<long, BoardMate> BoardMates { get; private set; } = new();
    public BoardMate? UserInfo { get; private set; }

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

        _hubConnection.On<NetworkUpdate>("ReceiveUpdate", HandleUpdate);
        _hubConnection.On<string>("ReceiveCursorUpdate", HandleCursorUpdate);
        _hubConnection.On<BoardMate>("ReceiveUserInfo", ReceiveUserInfo);
        _hubConnection.On<List<NetworkUpdate>>("InitialSync", HandleInitialSync);
        _hubConnection.On("AuthFailed", () => OnAuthFailed?.Invoke());
        _hubConnection.On("AccessDenied", () => OnAccessDenied?.Invoke());
        _hubConnection.On<BoardMate>("OnBoardMateJoined", OnBoardMateJoined);
        _hubConnection.On<long>("OnBoardMateLeft", OnBoardMateLeft);
        _hubConnection.Closed += HandleConnectionClosed;

        try
        {
            await _hubConnection.StartAsync();
            _isConnected = true;

            await _hubConnection.InvokeAsync("JoinBoard", _boardId, _authToken, 0 /*_lastUpdateId*/);

            _ = ProcessOutgoingQueue();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Connection failed: {ex.Message}");
        }
    }

    private void HandleCursorUpdate(string obj)
    {
        OnRemoteCursorUpdate?.Invoke(obj);
    }

    private void ReceiveUserInfo(BoardMate obj)
    {
        UserInfo = obj;
        OnReceiveUserInfo?.Invoke(obj);
    }

    private void OnBoardMateLeft(long obj)
    {
        BoardMates.Remove(obj, out _);
    }

    private void OnBoardMateJoined(BoardMate boardMate)
    {
        BoardMates.TryAdd(boardMate.UserId, boardMate);
        OnBoardMateJoin?.Invoke(boardMate);
    }

    public void QueueUpdate(NetworkUpdate networkUpdate)
    {
        _outgoingQueue.Enqueue(networkUpdate);
    }

    public void QueueCursorUpdate(string cursorData)
    {
        _outgoingCursorUpdatesQueue.Enqueue(cursorData);
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

                    // NetworkUpdate last processed ID
                    if (update.Id > _lastUpdateId)
                        _lastUpdateId = update.Id;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to send update: {ex.Message}");
                }
            }
            if (_outgoingCursorUpdatesQueue.TryDequeue(out var cursorUpdate))
            {
                try
                {
                    await _hubConnection!.InvokeAsync("SendCursorUpdate", cursorUpdate);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to send update: {ex.Message}");
                }
            }

            await Task.Delay(10);
        }
    }

    private void HandleUpdate(NetworkUpdate networkUpdate)
    {
        if (networkUpdate.Id <= _lastUpdateId) return;

        _lastUpdateId = networkUpdate.Id;
        OnUpdateReceived?.Invoke(networkUpdate);
    }

    private void HandleInitialSync(List<NetworkUpdate> updates)
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