using WeBoard.Client.WPF.Models;

namespace WeBoard.Client.WPF.Responses
{
    public class BoardApiResponse : BaseApiResponse
    {
        public Guid BoardId { get; set; }
        public List<BoardModel>? Boards { get; set; }
    }
}
