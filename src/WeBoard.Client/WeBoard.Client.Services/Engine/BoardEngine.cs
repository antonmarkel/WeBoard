using SFML.System;
using WeBoard.Client.Services.Interfaces.Base;
using WeBoard.Client.Services.Managers;
using WeBoard.Core.Components.Menu;

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
            ComponentManager.GetInstance();
            var random = new Random();

            var scrollView = new ScrollViewComponent(new Vector2f(100, 100), new Vector2f(600, 600));
            for(int i = 0; i < 10; i++)
            {
                scrollView.AddChild(new TextComponent(new Vector2f(0, i * 100), $"Hello kitties!{i}"));
            }


            MenuManager.GetInstance().Init([
                new TextComponent(new Vector2f(300,300), "Hello kitties!"),
                scrollView]);
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
