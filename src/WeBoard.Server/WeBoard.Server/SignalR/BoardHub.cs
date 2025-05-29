using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using WeBoard.Server.Persistence.Data;
using WeBoard.Server.Persistence.Entities;

public class BoardHub : Hub
{
    private readonly WeBoardContext _context;

    public BoardHub(WeBoardContext context)
    {
        _context = context;
    }

    private static readonly Dictionary<Guid, BoardRoom> _activeRooms = new();



    [HubMethodName("JoinBoard")]
    public async Task JoinBoard(Guid boardId, string authToken, long updateDateId)
    {
        var userId = await GetUserIdAsync(authToken);

        if (userId == -1)
        {
            await Clients.Caller.SendAsync("AuthFailed");
            Context.Abort();
            return;
        }

        if (!await _context.UserBoards.AnyAsync(ub => ub.BoardId == boardId && ub.UserId == userId))
        {
            await Clients.Caller.SendAsync("AccessDenied");
            return;
        }

        if (!_activeRooms.TryGetValue(boardId, out var room))
        {
            room = new BoardRoom(boardId);
            _activeRooms[boardId] = room;

            room.Updates = await _context.BoardUpdates.Where(up => up.BoardId == boardId).ToListAsync();
        }

        room.Connections.Add(Context.ConnectionId);
        await Groups.AddToGroupAsync(Context.ConnectionId, boardId.ToString());

        var updatesToSend =
            room.Updates.Where(u => u.Id > updateDateId).ToList();

        await Clients.Caller.SendAsync("InitialSync", updatesToSend);
    }

    [HubMethodName("SendUpdate")]
    public async Task SendUpdate(Guid boardId, BoardUpdateEntity update)
    {
        if (!_activeRooms.TryGetValue(boardId, out var room)) return;

        update.Id = DateTime.UtcNow.Ticks;
        room.Updates.Add(update);

        await Clients.All.SendAsync("ReceiveUpdate", update);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var roomsToLeave = _activeRooms.Values
            .Where(r => r.Connections.Contains(Context.ConnectionId))
            .ToList();

        foreach (var room in roomsToLeave)
        {
            room.Connections.Remove(Context.ConnectionId);

            if (room.Connections.Count == 0)
            {
                await _context.BoardUpdates.AddRangeAsync(room.Updates);
                await _context.SaveChangesAsync();
                _activeRooms.Remove(room.BoardId);
            }

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, room.BoardId.ToString());
        }

        await base.OnDisconnectedAsync(exception);
    }

    private async Task<long> GetUserIdAsync(string token)
    {

        if (!Guid.TryParse(token, out Guid tokenId))
        {
            return -1;
        }

        var tokenEntity = await _context.Tokens.FirstOrDefaultAsync(token => token.Id == tokenId);
        if (tokenEntity is null || tokenEntity.ValidTillUtc < DateTime.UtcNow)
        {
            return -1;
        }

        return tokenEntity.UserId;

    }
}

public class BoardRoom
{
    public Guid BoardId { get; }
    public List<BoardUpdateEntity> Updates { get; set; } = new();
    public HashSet<string> Connections { get; } = new();

    public BoardRoom(Guid boardId)
    {
        BoardId = boardId;
    }
}