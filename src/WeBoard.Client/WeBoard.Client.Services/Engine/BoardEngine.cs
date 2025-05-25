using WeBoard.Client.Services.Initializers;
using WeBoard.Client.Services.Interfaces.Base;
using WeBoard.Client.Services.Managers;
using WeBoard.Core.Components.Interfaces;
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

        public void Start()
        {
            _isRunning = true;
            _logicThread = new Thread(LogicLoop);
            _logicThread.Start();

            FocusManager.GetInstance();
            MouseManager.GetInstance();
            KeyboardManager.GetInstance();
            UpdateManager.GetInstance();
            MenuManager.GetInstance().CurrentInstrument = InstrumentOptionsEnum.Cursor;
            CursorManager.GetInstance();

            var menuInitializer = new MenuInitializer();
            var menuComponents = menuInitializer.InitializeComponents();

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

                var components = ComponentManager.GetInstance().GetComponentsForRender();
                foreach (var comp in components.ToList())
                {
                    if (comp is ICleanable cleanable && cleanable.ShouldBeClean)
                    {
                        ComponentManager.GetInstance().RemoveComponent(comp);
                        cleanable.ShouldBeClean = false;
                    }
                }

                UpdateManager.GetInstance().CollectUpdates();
                AnimationManager.GetInstance().OnUpdate((float)delta);
                OnUpdate?.Invoke();
                Thread.Sleep(16);
            }
        }
    }
}
