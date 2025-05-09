using SFML.System;
using SFML.Window;

namespace WeBoard.Client.Services.Managers
{
    public class MouseManager
    {
        private static readonly MouseManager Instance = new();
        private readonly RenderManager _global = RenderManager.GetInstance();
        private readonly FocusManager _focusManager = FocusManager.GetInstance();
        public bool IsDragging { get; set; }
        public Vector2i DragStartScreen { get; private set; }
        public Vector2f DragStartWorld { get; private set; }

        public MouseManager()
        {
            _global.RenderWindow.MouseButtonPressed += HandleMouseButtonPress;
            _global.RenderWindow.MouseButtonReleased += HandleMouseButtonReleased;
            _global.RenderWindow.MouseMoved += HandleMouseMove;
            _global.RenderWindow.MouseWheelScrolled += HandleMouseWheelScroll;
        }

        private void HandleMouseWheelScroll(object? sender, MouseWheelScrollEventArgs e)
        {
            _global.Camera.Zoom(e.Delta);

        }

        private void HandleMouseMove(object? sender, MouseMoveEventArgs e)
        {
            if (!IsDragging)
                return;

            var currentScreen = new Vector2i(e.X, e.Y);
            var currentWorld = _global.RenderWindow.MapPixelToCoords(currentScreen);
            var offsetScreen = DragStartScreen - currentScreen;
            var offsetWorld = DragStartWorld - currentWorld;

            if (_focusManager.FocusedComponent != null)
            {
                DragStartScreen = new Vector2i(e.X, e.Y);
                DragStartWorld = _global.RenderWindow.MapPixelToCoords(DragStartScreen);

                _focusManager.HandleDrag(-offsetWorld);

                return;
            }

            _global.Camera.MoveCamera(offsetWorld);

        }

        private void HandleMouseButtonReleased(object? sender, MouseButtonEventArgs e)
        {
            if (e.Button == Mouse.Button.Left)
            {
                IsDragging = false;
            }
        }

        private void HandleMouseButtonPress(object? sender, MouseButtonEventArgs e)
        {
            if (e.Button == Mouse.Button.Left)
            {
                IsDragging = true;
                DragStartScreen = new Vector2i(e.X, e.Y);
                DragStartWorld = _global.RenderWindow.MapPixelToCoords(DragStartScreen);

                FocusManager.GetInstance().HandleClick(DragStartWorld);
                if (FocusManager.GetInstance().FocusedComponent != null)
                    return;

            }
        }

        public static MouseManager GetInstance()
        {
            return Instance;
        }
    }
}
