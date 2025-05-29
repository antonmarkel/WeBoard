using Microsoft.EntityFrameworkCore;
using WeBoard.Server.Persistence.Entities;

namespace WeBoard.Server.Persistence.Data
{
    public class WeBoardContext : DbContext
    {
        public WeBoardContext(DbContextOptions<WeBoardContext> options) : base(options)
        {
            Database.AutoTransactionBehavior = AutoTransactionBehavior.Never;
            Database.EnsureCreated();
        }

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<BoardEntity> Boards { get; set; }
        public DbSet<BoardDataEntity> BoardsData { get; set; }
        public DbSet<BoardUpdateEntity> BoardUpdates { get; set; }
       
        public DbSet<TokenEntity> Tokens { get; set; }
        public DbSet<UserBoardEntity> UserBoards { get; set; }
        
    }
}
