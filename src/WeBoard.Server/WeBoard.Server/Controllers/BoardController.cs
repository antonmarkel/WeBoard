using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeBoard.Server.Models;
using WeBoard.Server.Persistence.Data;
using WeBoard.Server.Persistence.Entities;

namespace WeBoard.Server.Controllers
{
    [ApiController]
    [Route("boards")]
    public class BoardController : ControllerBase
    {
        private readonly WeBoardContext _context;

        public BoardController(WeBoardContext context)
        {
            _context = context;
        }

        private async Task<UserEntity?> IsAuthorizedAsync(Guid token)
        {
            var tokenEntity = await _context.Tokens.FirstOrDefaultAsync(tok => tok.Id == token);
            if (tokenEntity is null || tokenEntity.ValidTillUtc < DateTime.UtcNow)
                return null;

            var userEntity = await _context.Users.FirstOrDefaultAsync(user => user.Id == tokenEntity.UserId);
            return userEntity;
        }

        [HttpPost("{token}")]
        public async Task<IActionResult> CreateNewBoardAsync([FromBody] CreateBoardRequest request, [FromRoute] Guid token)
        {
            var userEntity = await IsAuthorizedAsync(token);
            if (userEntity is null)
                return BadRequest("Not authorized");

            var boardEntity = new BoardEntity
            {
                Id = Guid.NewGuid(),
                LastOpenedAtUtc = DateTime.UtcNow,
                LastUpdatedAtUtc = DateTime.UtcNow,
                Name = request.Name,
            };
            var userBoard = new UserBoardEntity
            {
                BoardId = boardEntity.Id,
                UserId = userEntity.Id,
                Id = Guid.NewGuid()
            };

            await _context.Boards.AddAsync(boardEntity);
            await _context.UserBoards.AddAsync(userBoard);
            await _context.SaveChangesAsync();

            return Ok(boardEntity.Id);
        }

        [HttpPost("add/{boardId}/{token}")]
        public async Task<IActionResult> AddBoardAsync([FromRoute] Guid boardId, [FromRoute] Guid token)
        {
            var userEntity = await IsAuthorizedAsync(token);
            if (userEntity is null)
                return BadRequest("Not authorized!");

            var boardEntity = await _context.Boards.FirstOrDefaultAsync(board => board.Id == boardId);
            if (boardEntity is null)
                return NotFound();

            var userBoard = new UserBoardEntity
            {
                BoardId = boardEntity.Id,
                UserId = userEntity.Id,
                Id = Guid.NewGuid()
            };
            await _context.UserBoards.AddAsync(userBoard);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("{boardId}/{token}")]
        public async Task<IActionResult> GetAllBoardUsersAsync([FromRoute] Guid boardId, [FromRoute] Guid token)
        {
            var userEntity = await IsAuthorizedAsync(token);
            if (userEntity is null)
                return BadRequest("Not authorized!");

            var board = await _context.Boards.FirstOrDefaultAsync(b => b.Id == boardId);
            if (board is null)
                return BadRequest();

            var userIds =await _context.UserBoards.Where(ub => ub.BoardId == boardId).Select(ub => ub.UserId).ToListAsync();
            var names = await _context.Users.Where(u => userIds.Contains(u.Id)).Select(u => u.UserName).ToListAsync();

            return Ok(names);

        }
    }
}
