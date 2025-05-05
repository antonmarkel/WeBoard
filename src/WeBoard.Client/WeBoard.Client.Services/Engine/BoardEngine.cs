using WeBoard.Client.Services.Interfaces.Base;

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

        public void Start()
        {
            _isRunning = true;
            _logicThread = new Thread(LogicLoop);
            _logicThread.Start();
        }

        public void Stop()
        {
            _isRunning = false;
            _logicThread?.Join();
        }

        private void LogicLoop()
        {
            while (_isRunning)
            {
                OnUpdate?.Invoke();
                Thread.Sleep(16);
            }
        }
    }
}
