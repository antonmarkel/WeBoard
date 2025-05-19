using SFML.Graphics;
using WeBoard.Client.Services.Interfaces.Base;
using WeBoard.Client.Services.Managers;
using WeBoard.Core.Components.Content;
using WeBoard.Core.Components.Menu.Buttons;

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

            FocusManager.GetInstance();
            MouseManager.GetInstance();
            KeyboardManager.GetInstance();
            var imageContent = new ImageContentView(new Texture("Resources/Handlers/Arrow.png"));
            ComponentManager.GetInstance().InitMenu([new ButtonComponent(new SFML.System.Vector2f(100, 100), new SFML.System.Vector2f(400, 100))
            {
                ContentView = imageContent
            }]);
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
