namespace WeBoard.Server.Persistence.Entities
{
    public class UserBoardEntity
    {
        public Guid Id { get; set; }
        public long UserId { get; set; }
        public Guid BoardId { get; set; }
    }
}
