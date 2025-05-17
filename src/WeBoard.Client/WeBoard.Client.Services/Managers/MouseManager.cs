using SFML.System;
using SFML.Window;
using WeBoard.Core.Components.Interfaces;

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
            var currentScreen = new Vector2i(e.X, e.Y);
            var currentWorld = _global.RenderWindow.MapPixelToCoords(currentScreen);
            var offsetScreen = DragStartScreen - currentScreen;
            var offsetWorld = DragStartWorld - currentWorld;

            ToolManager.GetInstance().OnMouseMoved(currentWorld);

            if (!IsDragging)
                return;

          

            if (_focusManager.ActiveHandler is IDraggable draggable)
            {
                DragStartScreen = new Vector2i(e.X, e.Y);
                DragStartWorld = _global.RenderWindow.MapPixelToCoords(DragStartScreen);

                draggable.Drag(-offsetWorld);

                return;
            }

            if (_focusManager.FocusedComponent != null)
            {
                DragStartScreen = new Vector2i(e.X, e.Y);
                DragStartWorld = _global.RenderWindow.MapPixelToCoords(DragStartScreen);

                if (_focusManager.FocusedComponent is not IDraggable)
                    return;

                ((IDraggable)_focusManager.FocusedComponent).Drag(-offsetWorld);

                return;
            }

            _global.Camera.MoveCamera(offsetWorld);

        }

        private void HandleMouseButtonReleased(object? sender, MouseButtonEventArgs e)
        {
            if (e.Button == Mouse.Button.Left && Keyboard.IsKeyPressed(Keyboard.Key.LControl))
            {
                ToolManager.GetInstance().OnMouseReleased(_global.RenderWindow.MapPixelToCoords(new Vector2i(e.X, e.Y)));
                return;
            }

            if (e.Button == Mouse.Button.Left)
            {
                IsDragging = false;   
            }
        }

        private void HandleMouseButtonPress(object? sender, MouseButtonEventArgs e)
        {
            if (e.Button == Mouse.Button.Left && Keyboard.IsKeyPressed(Keyboard.Key.LControl))
            {
                DragStartScreen = new Vector2i(e.X, e.Y);
                DragStartWorld = _global.RenderWindow.MapPixelToCoords(DragStartScreen);

                ToolManager.GetInstance().OnMousePressed(DragStartWorld);
                return;
            }


            if (e.Button == Mouse.Button.Left)
            {
                IsDragging = true;
                DragStartScreen = new Vector2i(e.X, e.Y);
                DragStartWorld = _global.RenderWindow.MapPixelToCoords(DragStartScreen);

                var menuClickedComponent = ComponentManager.GetInstance().GetMenuComponents()
                    .FirstOrDefault(comp => comp.Intersect(DragStartScreen, out _));
                if (menuClickedComponent is IClickable clickable)
                {
                    clickable.OnClick();
                }

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
