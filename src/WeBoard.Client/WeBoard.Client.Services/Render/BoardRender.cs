using System.Numerics;
using SFML.Graphics;
using SFML.System;
using WeBoard.Client.Services.Managers;
using WeBoard.Core.Drawables.Cursors;

namespace WeBoard.Client.Services.Render
{
    public sealed class BoardRender
    {
        private bool _isRunning = false;
        public bool IsRunning => _isRunning;
        private readonly RenderManager _global = RenderManager.GetInstance();
        private readonly ComponentManager _componentManager = ComponentManager.GetInstance();
        private readonly CursorManager _cursorManager = CursorManager.GetInstance();
        public BoardRender(RenderWindow window)
        {
            _global.RenderWindow = window;
            _global.RenderWindow.Closed += (_, _) => _isRunning = false;
        }

        public void Start()
        {
            _isRunning = true;
            var window = _global.RenderWindow;
            while (_isRunning)
            {
                window!.SetView(_global.Camera.CameraView);
                window.DispatchEvents();

                window.Clear(new Color(255, 238, 242));
                var drawables = _componentManager.GetComponentsForRender();
                foreach (var renderObject in drawables)
                {
                    lock (renderObject)
                    {
                        if (renderObject.Parent is null)
                            window.Draw(renderObject);
                    }
                }
                RemoteCursorManager.GetInstance().Draw(window,RenderStates.Default);
                ToolManager.GetInstance().Draw(window, RenderStates.Default);

                window.SetView(_global.Camera.UiView);
                var menuObjects = _componentManager.GetMenuComponents();
                foreach (var menuObject in menuObjects)
                {
                    lock (menuObject)
                    {
                        if (menuObject.Parent is null)
                            window.Draw(menuObject);
                    }
                }

                if (EditManager.GetInstance().CurrentEditContainer != null) { }
                window.Draw(_cursorManager);


                window.Display();
            }
        }
    }
}
