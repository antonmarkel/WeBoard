

namespace WeBoard.Core.Network.Dtos
{
    public class Update
    {
        public long Id { get; set; }
        public Guid BoardId { get; set; }
        public string Data { get; set; } = string.Empty;
    }
}
