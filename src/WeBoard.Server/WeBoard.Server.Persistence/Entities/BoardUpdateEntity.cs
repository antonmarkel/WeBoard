using System.ComponentModel.DataAnnotations;

namespace WeBoard.Server.Persistence.Entities
{
    public class BoardUpdateEntity
    {
        /// <summary>
        /// //Time
        /// </summary>
        public long Id { get; set; }
        public Guid BoardId { get; set; }
        public string Data { get; set; } = string.Empty;
    }
}
