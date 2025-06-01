using System.ComponentModel.DataAnnotations;

namespace WeBoard.Server.Persistence.Entities
{
    public class UserEntity
    {
        public long Id { get; set; }

        [MaxLength(254)]
        public string EncryptedPassword { get; set; } = string.Empty;

        [MaxLength(100)] public string UserName { get; set; } = string.Empty;

        public virtual ICollection<BoardEntity> Boards { get; set; } = [];  

    }
}
