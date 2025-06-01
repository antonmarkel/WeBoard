namespace WeBoard.Server.Persistence.Entities
{
    public class TokenEntity
    {
        public Guid Id { get; set; }
        public long UserId { get; set; }
        public DateTime ValidTillUtc { get; set; }
    }
}
