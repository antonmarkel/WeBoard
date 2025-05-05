using System.Collections.Immutable;
using SFML.Graphics;
using WeBoard.Client.Core.Engine;

namespace WeBoard.Client.Services.Render
{
    public sealed class BoardRender
    {
        private bool _isRunning = false;
        public bool IsRunning => _isRunning;
        private BoardCamera _camera;
        private readonly BoardGlobal _global = BoardGlobal.GetInstance();
        public BoardRender(RenderWindow window)
        {
            _global.RenderWindow = window;
            _global.RenderWindow.Closed += (_, _) => _isRunning = false;
            _camera = new BoardCamera(window);
        }

        public void Start()
        {
            _isRunning = true;
            var window = _global.RenderWindow;
            while (_isRunning)
            {
                window!.SetView(_camera.CameraView);
                window.DispatchEvents();

                window.Clear(new Color(255, 238, 242));
                var drawables = _global.RenderObjects.Values.ToImmutableArray();
                foreach (var renderObject in drawables)
                {
                    lock (renderObject)
                    {
                        window.Draw(renderObject);
                    }
                }
                window.Display();

            }
        }
    }
}
