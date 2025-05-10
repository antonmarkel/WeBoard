using SFML.System;
using System.Drawing;
using WeBoard.Client.Services.Interfaces.Base;
using WeBoard.Client.Services.Managers;
using WeBoard.Core.Components.Base;
using WeBoard.Core.Components.Menu;
using WeBoard.Core.Components.Menu.Scrolls;

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

            var scrollChildern = new List<MenuComponentBase>();
            var childScrollChildren = new List<MenuComponentBase>();
            for (int i = 0; i < 20; i++)
            {
                scrollChildern.Add(new TextComponent(new Vector2f(20, i * 100), $"Hello kitties!{i}")
                {
                    Size = 14
                });
                childScrollChildren.Add(new TextComponent(new Vector2f(20, i * 100), $"Hello kitties!{i}")
                {
                    Size = 14
                });
            }

            var scrollView2 = new ScrollViewComponent(new Vector2f(100, 100), new Vector2f(300, 600), childScrollChildren, true, 20)
            {
                BackgroundColor = new SFML.Graphics.Color(50, 50, 50),
                IsVisible = true
            };
            scrollChildern.Add(scrollView2);

            var scrollView = new ScrollViewComponent(new Vector2f(100, 100), new Vector2f(300, 600), scrollChildern, true, 20)
            {
                BackgroundColor = new SFML.Graphics.Color(50, 50, 50),
                IsVisible = true
            };
           

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
