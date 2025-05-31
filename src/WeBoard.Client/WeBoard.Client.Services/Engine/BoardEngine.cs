using WeBoard.Client.Services.Initializers;
using WeBoard.Client.Services.Interfaces.Base;
using WeBoard.Client.Services.Managers;
using WeBoard.Core.Enums.Menu;

namespace WeBoard.Client.Services.Engine
{
    public class BoardEngine
    {
        private bool _isRunning;
        private Thread? _logicThread;

        public delegate void BoardUpdateHandler();
        public event BoardUpdateHandler? OnUpdate;

        public void AddService(IService service)
        {
            OnUpdate += service.OnUpdate;
        }

        private void InitNetwork()
        {
            
        }

        public void Start()
        {
            _isRunning = true;
            _logicThread = new Thread(LogicLoop);
            _logicThread.Start();

            FocusManager.GetInstance();
            MouseManager.GetInstance();
            KeyboardManager.GetInstance();
            UpdateManager.GetInstance();
            ToolManager.GetInstance();
            MenuManager.GetInstance().CurrentInstrument = InstrumentOptionsEnum.Cursor;
            CursorManager.GetInstance();
            EditManager.GetInstance();
            var networkManager = NetworkManager.GetInstance();

            networkManager.AuthToken = "7e698b12-1fec-458c-ae0c-2a3005df8a02";
            Guid.TryParse("329cad88-8386-46de-8f1d-cf59ccc931d8", out Guid boardId);
                networkManager.BoardId = boardId;
            networkManager.ServerUrl = "http://localhost:51264";
            networkManager.LastUpdateId = 0;

            var menuInitializer = new MenuInitializer();
            var menuComponents = menuInitializer.InitializeComponents();

            ComponentManager.GetInstance().InitMenu(menuComponents);
            networkManager.Start();
        }

        public void Stop()
        {
            _isRunning = false;
            _logicThread?.Join();
        }

        private void LogicLoop()
        {
            var lastTime = DateTime.Now;
            while (_isRunning)
            {
                var currentTime = DateTime.Now;
                var delta = (currentTime - lastTime).TotalMilliseconds;
                lastTime = currentTime;
                UpdateManager.GetInstance().CollectUpdates();
                AnimationManager.GetInstance().OnUpdate((float)delta);
                OnUpdate?.Invoke();
                Thread.Sleep(16);
            }
        }
    }
}
