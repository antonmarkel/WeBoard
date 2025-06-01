namespace WeBoard.Server.Models
{
    public class RegisterRequest
    {
        public required string Name { get; set; }
        public required string Password { get; set; }
    }
}
