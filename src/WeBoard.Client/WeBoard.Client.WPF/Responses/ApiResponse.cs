namespace WeBoard.Client.WPF.Responses
{
    public class ApiResponse
    {
        public bool Success { get; set; } = true;
        public string? Message { get; set; }
        public string? Id { get; set; }
    }
}
