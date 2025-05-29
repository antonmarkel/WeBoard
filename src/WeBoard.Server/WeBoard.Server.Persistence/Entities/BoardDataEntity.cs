namespace WeBoard.Server.Persistence.Entities
{
    public class BoardDataEntity
    {
        public long Id { get; set; } 
        public virtual ICollection<BoardUpdateEntity> BoardUpdates { get; set; } = [];
    }
}
