namespace WeBoard.Client.WPF.Responses
{
    public class BaseApiResponse
    {
        public bool Success { get; set; } = true;
        public string? Message { get; set; }
    }
}
