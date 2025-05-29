using DigiHelper.Logic.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeBoard.Server.Models;
using WeBoard.Server.Persistence.Data;
using WeBoard.Server.Persistence.Entities;

namespace WeBoard.Server.Controllers
{
    [ApiController]
    [Route("users")]
    public class UserController : ControllerBase
    {
        private readonly WeBoardContext _context;

        public UserController(WeBoardContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterRequest request)
        {
            var entity = new UserEntity
            {
                Id = new Random().NextInt64(),
                UserName = request.Name,
                EncryptedPassword = PasswordHelper.HashPassword(request.Password)
            };

            var old = await _context.Users.AnyAsync(us => us.UserName == request.Name);
            if (old)
                return BadRequest("Username is already in use!");

            await _context.Users.AddAsync(entity);
            await _context.SaveChangesAsync();

            return Created();
        }

        [HttpPost("login")]
        public async Task<IActionResult> LogicAsync([FromBody] RegisterRequest request)
        {
            var userEntity = await _context.Users.FirstOrDefaultAsync(u => u.UserName == request.Name);
            if (userEntity is null)
                return NotFound();

            if (!PasswordHelper.VerifyPassword(request.Password, userEntity.EncryptedPassword))
                return BadRequest("Wrong password!");


            var tokenEntity = new TokenEntity
            {
                Id = Guid.NewGuid(),
                ValidTillUtc = DateTime.UtcNow.AddHours(3),
                UserId = userEntity.Id
            };
            
            await _context.Tokens.AddAsync(tokenEntity);
            await _context.SaveChangesAsync();

            return Ok(tokenEntity.Id);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _context.Users.Select(us => us.UserName).ToListAsync();
            return Ok(result);
        }
    }
}
