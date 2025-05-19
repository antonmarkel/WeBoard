using SFML.Graphics;
using WeBoard.Client.Services.Interfaces.Base;
using WeBoard.Client.Services.Managers;
using WeBoard.Core.Animations;
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
            var imageContent = new ImageContentView(new Texture("Resources/Handlers/Arrow.png"));
            var button = new ButtonComponent(new SFML.System.Vector2f(100, 100), new SFML.System.Vector2f(400, 100))
            {
                ContentView = imageContent
            };

            ComponentManager.GetInstance().InitMenu([button]);
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

                AnimationManager.GetInstance().OnUpdate((float)delta);
                OnUpdate?.Invoke();
                Thread.Sleep(16);
            }
        }
    }
}
