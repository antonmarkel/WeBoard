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
        var user = await GetUserAsync(authToken);

        if (user is null)
        {
            await Clients.Caller.SendAsync("AuthFailed");
            Context.Abort();
            return;
        }

        if (!await _context.UserBoards.AnyAsync(ub => ub.BoardId == boardId && ub.UserId == user.Id))
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

        if (!room.Users.Any(u => u.UserId == user.Id))
            room.Users.Add(new BoardMate { Name = user.UserName, UserId = user.Id });

        await Groups.AddToGroupAsync(Context.ConnectionId, boardId.ToString());

        var updatesToSend =
            room.Updates.Where(u => u.Id > updateDateId).ToList();

        await Clients.Caller.SendAsync("ReceiveUserInfo", new BoardMate { Name = user.UserName, UserId = user.Id });
        await Clients.Caller.SendAsync("InitialSync", updatesToSend);

        foreach (var mate in room.Users)
        {
            if(mate.UserId != user.Id)
                await Clients.Caller.SendAsync("OnBoardMateJoined", mate);
        }

        await Clients.Others.SendAsync("OnBoardMateJoined",
            new BoardMate { Name = user.UserName, UserId = user.Id });
    }

    [HubMethodName("SendUpdate")]
    public async Task SendUpdate(Guid boardId, BoardUpdateEntity update)
    {
        if (!_activeRooms.TryGetValue(boardId, out var room)) return;

        update.Id = DateTime.UtcNow.Ticks;
        room.Updates.Add(update);

        await Clients.All.SendAsync("ReceiveUpdate", update);
    }

    [HubMethodName("SendCursorUpdate")]
    public async Task SendCursorUpdate(string cursorData)
    {
        await Clients.Others.SendAsync("ReceiveCursorUpdate", cursorData);
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
                foreach (var update in room.Updates)
                {
                    if (!await _context.BoardUpdates.AnyAsync(up => up.Id == update.Id))
                    {
                        await _context.BoardUpdates.AddAsync(update);
                    }
                }
                await _context.SaveChangesAsync();
                _activeRooms.Remove(room.BoardId);
            }

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, room.BoardId.ToString());
        }

        await base.OnDisconnectedAsync(exception);
    }

    private async Task<UserEntity?> GetUserAsync(string token)
    {

        if (!Guid.TryParse(token, out Guid tokenId))
        {
            return null;
        }

        var tokenEntity = await _context.Tokens.FirstOrDefaultAsync(token => token.Id == tokenId);
        if (tokenEntity is null || tokenEntity.ValidTillUtc < DateTime.UtcNow)
        {
            return null;
        }

        var userEntity = await _context.Users.FirstOrDefaultAsync(u => u.Id == tokenEntity.UserId);

        return userEntity;
    }
}

public class BoardMate
{
    public long UserId { get; set; }
    public string Name { get; set; } = string.Empty;
}
public class BoardRoom
{
    public Guid BoardId { get; }
    public List<BoardUpdateEntity> Updates { get; set; } = new();
    public List<BoardMate> Users { get; set; } = new();
    public HashSet<string> Connections { get; } = new();

    public BoardRoom(Guid boardId)
    {
        BoardId = boardId;
    }
}