using System.ComponentModel.DataAnnotations;

namespace WeBoard.Server.Persistence.Entities
{
    public class BoardUpdateEntity
    {
        /// <summary>
        /// //Time
        /// </summary>
        [Key]
        public long DateId { get; set; }
        public Guid BoardId { get; set; }
        public string Data { get; set; } = string.Empty;
    }
}
