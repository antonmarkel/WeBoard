namespace WeBoard.Server.Configuration
{
    public class MongoDbSettings
    {
        public required string DefaultConnection { get; set; }
        public required string DefaultDatabaseName { get; set; }
    }
}
