using WeBoard.Core.Network.Dtos;
using WeBoard.Core.Network.Serializable.Tools;
using WeBoard.Core.Updates.Interfaces;

namespace WeBoard.Client.Services.Managers
{
    public class NetworkManager
    {
        private static NetworkManager? Instance;
        private BoardHubClient _hubClient;
        private HashSet<long> _handledUpdatesDateTicks = new();
        public string ServerUrl { get; set; } = string.Empty;
        public string AuthToken { get; set; } = string.Empty;
        public Guid BoardId { get; set; }
        public long LastUpdateId { get; set; }

        public static NetworkManager GetInstance()
        {
            return Instance ?? (Instance = new());
        }

        private NetworkManager()
        {
            
        }

        public void Start()
        {
            _hubClient = new BoardHubClient($"{ServerUrl}/boardHub", AuthToken, BoardId, LastUpdateId);

            _hubClient.OnInitialSync += HandleInitialSync;
            _hubClient.OnUpdateReceived += HandleUpdate;
            _hubClient.OnAuthFailed += HandleAuthFailure;
            _hubClient.OnAccessDenied += HandleAccessDenied;
            _hubClient.OnConnectionClosed += HandleDisconnect;

            _hubClient.ConnectAsync();
        }

        public void SendUpdate(IUpdate update)
        {
            var networkUpdate = new NetworkUpdate
            {
                BoardId = BoardId,
                Data = update.Serialize(),
                Id = DateTime.UtcNow.Ticks
            };
            _handledUpdatesDateTicks.Add(update.Date.Ticks);
            _hubClient.QueueUpdate(networkUpdate);
        }

        private void HandleDisconnect()
        {
            Console.WriteLine("ERROR! Connection failed!");
        }

        private void HandleAccessDenied()
        {
            Console.WriteLine("ERROR! Access denied!");
        }

        private void HandleAuthFailure()
        {
            Console.WriteLine("ERROR! Auth failure!");
        }

        private void HandleUpdate(NetworkUpdate networkUpdate)
        {
            var update = UpdateSerializer.Deserialize(networkUpdate.Data);
            if (update is null)
            {
                Console.WriteLine("Update receiving error!");
                return;
            }

            if (_handledUpdatesDateTicks.Contains(update.Date.Ticks))
                return;

            UpdateManager.GetInstance().ApplyUpdate(update);
        }

        private void HandleInitialSync(List<NetworkUpdate> list)
        {
            foreach (var update in list)
            {
                HandleUpdate(update);
            }
        }
        
    }
}
