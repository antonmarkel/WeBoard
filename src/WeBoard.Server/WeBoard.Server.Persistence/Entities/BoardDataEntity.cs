namespace WeBoard.Server.Persistence.Entities
{
    public class BoardDataEntity
    {
        public long Id { get; set; } 
        public List<BoardUpdateEntity> BoardUpdates { get; set; } = [];
    }
}
