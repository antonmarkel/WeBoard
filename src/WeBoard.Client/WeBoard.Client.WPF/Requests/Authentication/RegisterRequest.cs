namespace WeBoard.Client.WPF.Requests.Authentication
{
    public class RegisterRequest
    {
        public required string Name { get; set; }
        public required string Password { get; set; }
    }
}
