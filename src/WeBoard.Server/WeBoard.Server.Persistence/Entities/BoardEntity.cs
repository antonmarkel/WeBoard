namespace WeBoard.Server.Persistence.Entities
{
    public class BoardEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime LastOpenedAtUtc { get; set; }
        public DateTime LastUpdatedAtUtc { get; set; }
    }
}
