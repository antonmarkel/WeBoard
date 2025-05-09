using SFML.System;
using SFML.Window;
using WeBoard.Core.Components.Interfaces;

namespace WeBoard.Client.Services.Managers
{
    public class MouseManager
    {
        private static readonly MouseManager Instance = new();
        private readonly RenderManager _renderManager = RenderManager.GetInstance();
        private readonly FocusManager _focusManager = FocusManager.GetInstance();
        private readonly ComponentManager _componentManager = ComponentManager.GetInstance();
        public bool IsDragging { get; set; }
        public Vector2i DragStartScreen { get; private set; }
        public Vector2f DragStartWorld { get; private set; }

        public MouseManager()
        {
            _renderManager.RenderWindow.MouseButtonPressed += HandleMouseButtonPress;
            _renderManager.RenderWindow.MouseButtonReleased += HandleMouseButtonReleased;
            _renderManager.RenderWindow.MouseMoved += HandleMouseMove;
            _renderManager.RenderWindow.MouseWheelScrolled += HandleMouseWheelScroll;
        }

        private void HandleMouseWheelScroll(object? sender, MouseWheelScrollEventArgs e)
        {
            if(_focusManager.UnderMouse != null && _focusManager.UnderMouse is IScrollable)
            {
                ((IScrollable)_focusManager.UnderMouse).Scroll(e.Delta);

                return;
            }

            _renderManager.Camera.Zoom(e.Delta);

        }

        private void HandleMouseOver(Vector2i screen, Vector2f worlds)
        {
            var component = _componentManager.GetByPoints(worlds, screen);
            if (component != null && component is IMouseDetective)
            {
                if (_focusManager.UnderMouse != component)
                    _focusManager.UnderMouse?.OnMouseLeave();

                _focusManager.UnderMouse = (IMouseDetective)component;
                _focusManager.UnderMouse.OnMouseOver();

                return;
            }

            _focusManager.UnderMouse?.OnMouseLeave();
            _focusManager.UnderMouse = null;
        }

        private void HandleMouseMove(object? sender, MouseMoveEventArgs e)
        {
            var currentScreen = new Vector2i(e.X, e.Y);
            var currentWorld = _renderManager.RenderWindow.MapPixelToCoords(currentScreen);
            HandleMouseOver(currentScreen, currentWorld);
            if (!IsDragging)
            {

                return;
            }

            var offsetScreen = DragStartScreen - currentScreen;
            var offsetWorld = DragStartWorld - currentWorld;

            if (_focusManager.FocusedComponent != null)
            {
                DragStartScreen = new Vector2i(e.X, e.Y);
                DragStartWorld = _renderManager.RenderWindow.MapPixelToCoords(DragStartScreen);

                if (_focusManager.FocusedComponent is not IDraggable)
                {
                    _renderManager.Camera.MoveCamera(offsetWorld);
                    return;
                }

                ((IDraggable)_focusManager.FocusedComponent).Drag(-offsetWorld);

                return;
            }

            _renderManager.Camera.MoveCamera(offsetWorld);

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
                DragStartWorld = _renderManager.RenderWindow.MapPixelToCoords(DragStartScreen);

                FocusManager.GetInstance().HandleClick(DragStartWorld, DragStartScreen);
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
