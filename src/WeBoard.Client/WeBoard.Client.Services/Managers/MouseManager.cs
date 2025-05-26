using SFML.System;
using SFML.Window;
using WeBoard.Core.Components.Interfaces;
using WeBoard.Core.Components.Visuals;

namespace WeBoard.Client.Services.Managers
{
    public class MouseManager
    {
        private static readonly MouseManager Instance = new();
        private readonly RenderManager _global = RenderManager.GetInstance();
        private readonly FocusManager _focusManager = FocusManager.GetInstance();
        private readonly CursorManager _cursorManager = CursorManager.GetInstance();
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

        private void HandleMouseOver(Vector2i currentScreen)
        {
            var underMouse = ComponentManager.GetInstance().GetByScreenPoint(currentScreen, out _);
            if (underMouse is null)
            {
                if(_focusManager.UnderMouse != null)
                    _focusManager.UnderMouse.OnMouseLeave();
                _focusManager.UnderMouse = underMouse;

                return;
            }

            if (underMouse != _focusManager.UnderMouse)
            {
                _focusManager.UnderMouse?.OnMouseLeave();

                _focusManager.UnderMouse = underMouse;
                underMouse.OnMouseOver();
            }

        }

        private void HandleMouseMove(object? sender, MouseMoveEventArgs e)
        {

            var currentScreen = new Vector2i(e.X, e.Y);
            _cursorManager.SetPosition(currentScreen);
            var currentWorld = _global.RenderWindow.MapPixelToCoords(currentScreen);
            var offsetScreen = DragStartScreen - currentScreen;
            var offsetWorld = DragStartWorld - currentWorld;

            HandleMouseOver(currentScreen);

            if (!IsDragging)
            {
                if(_focusManager.FocusedComponent is IDraggable dragComponent)
                    dragComponent.OnStopDragging();
                return;
            }

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

                Vector2f clickOffset = new Vector2f(0, 0);
                var menuClickedComponent = ComponentManager.GetInstance().GetMenuComponents()
                    .FirstOrDefault(comp => comp.Intersect(DragStartScreen, out clickOffset));
                if (menuClickedComponent is IClickable clickable)
                {
                    clickable.OnClick(-clickOffset);
                    FocusManager.GetInstance().UpdateFocus(menuClickedComponent);
                    menuClickedComponent.OnFocus();

                    return;
                }

                if (KeyboardManager.GetInstance().IsInTextMode())
                {
                    var textComponent = new TextComponent(DragStartWorld);
                    ComponentManager.GetInstance().AddComponent(textComponent);
                    FocusManager.GetInstance().HandleClick(textComponent.Position);

                    textComponent.StartEditing();
                    KeyboardManager.GetInstance().ExitTextMode();
                    return;
                }

                FocusManager.GetInstance().HandleClick(DragStartWorld);
                if (FocusManager.GetInstance().FocusedComponent != null)
                    return;

            }
            if (e.Button == Mouse.Button.Right)
            {
                EditManager.GetInstance().CurrentEditContainer?.Hide();
                ComponentManager.GetInstance().RemoveMenuComponent(EditManager.GetInstance().CurrentEditContainer!);
                if (_focusManager.FocusedComponent is IEditable editable)
                {
                    EditManager.GetInstance().UpdateEditPanel(editable, new Vector2f(e.X, e.Y));
                    ComponentManager.GetInstance().AddMenuComponent(EditManager.GetInstance().CurrentEditContainer!);
                }
            }
        }

        public static MouseManager GetInstance()
        {
            return Instance;
        }
    }
}
