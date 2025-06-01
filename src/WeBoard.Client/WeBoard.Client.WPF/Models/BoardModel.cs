namespace WeBoard.Client.WPF.Models
{
    public class BoardModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsFavorite { get; set; } = false;
    }
}
