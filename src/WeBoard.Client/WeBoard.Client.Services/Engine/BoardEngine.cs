using SFML.Graphics;
using SFML.System;
using WeBoard.Client.Services.Interfaces.Base;
using WeBoard.Client.Services.Managers;
using WeBoard.Core.Animations;
using WeBoard.Core.Components.Base;
using WeBoard.Core.Components.Content;
using WeBoard.Core.Components.Menu.Buttons;
using WeBoard.Core.Components.Menu.Containers;

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
            UpdateManager.GetInstance();
            var imageContent = new ImageContentView(new Texture("Resources/Handlers/Arrow.png"));
            List<MenuComponentBase> menuComponents = new List<MenuComponentBase>();
            List<ButtonComponent> buttons = new ();
            for (int i = 0; i < 5; i++)
            {
                buttons.Add(new ButtonComponent(new SFML.System.Vector2f(0, 0), new SFML.System.Vector2f(50, 50))
                {
                    BackgroundColor = new Color(255,255,255,255),
                    ContentView = imageContent,
                    CornerRadius = 5,
                    CornerPointCount = 20,
                    OutlineThickness = 1
                });
                menuComponents.Add(buttons[i]);
            }

            var verticalStack = new VerticalStackContainer(buttons)
            {
                BackgroundColor = new Color(255, 255, 255, 80),
                OutlineThickness = 1f,
                OutlineColor = Color.Black,
                Padding = new Vector2f(15, 7),
                SpaceBetween = 15f,
                Position = new Vector2f(-10, 200),
                CornerRadius = 10,
                CornerPointCount = 40
            };
            menuComponents.Add(verticalStack);

            ComponentManager.GetInstance().InitMenu(menuComponents);
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
