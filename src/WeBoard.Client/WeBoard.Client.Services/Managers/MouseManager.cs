using SFML.System;
using SFML.Window;
using WeBoard.Core.Components.Base;
using WeBoard.Core.Components.Interfaces;
using WeBoard.Core.Components.Tools;
using WeBoard.Core.Components.Visuals;
using WeBoard.Core.Enums.Menu;
using WeBoard.Core.Network.Serializable.Tools;
using WeBoard.Core.Updates.Creation;

namespace WeBoard.Client.Services.Managers
{
    public class MouseManager
    {
        private static readonly MouseManager Instance = new();
        private readonly RenderManager _global = RenderManager.GetInstance();
        private readonly FocusManager _focusManager = FocusManager.GetInstance();
        private readonly CursorManager _cursorManager = CursorManager.GetInstance();
        private readonly ToolManager _toolManager = ToolManager.GetInstance();
        private readonly ShapeManager _shapeManager = ShapeManager.GetInstance();

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
                if (_focusManager.UnderMouse != null)
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
            RemoteCursorManager.GetInstance().UpdateUserCursor(currentWorld);
            ToolManager.GetInstance().OnMouseMoved(currentWorld);

            if (!IsDragging)
                return;
            HandleMouseOver(currentScreen);

            if (!IsDragging)
            {
                if (_focusManager.FocusedComponent is IDraggable dragComponent)
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
                var currentInstrument = MenuManager.GetInstance().CurrentInstrument;

                if (currentInstrument is InstrumentOptionsEnum.Brush
                    or InstrumentOptionsEnum.Pencil
                    or InstrumentOptionsEnum.Eraser)
                {
                    ToolManager.GetInstance().OnMouseReleased(_global.RenderWindow.MapPixelToCoords(new Vector2i(e.X, e.Y)));
                }

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

                var currentInstrument = MenuManager.GetInstance().CurrentInstrument;
                _toolManager.UpdateToolFromMenu();
                if (currentInstrument == InstrumentOptionsEnum.Text)
                {
                    var textComponent = new TextComponent(DragStartWorld);
                    ComponentManager.GetInstance().AddComponent(textComponent);
                    _focusManager.HandleClick(textComponent.Position);
                    textComponent.StartEditing();
                    KeyboardManager.GetInstance().ExitTextMode();
                    MenuManager.GetInstance().CurrentInstrument = InstrumentOptionsEnum.Cursor;
                    return;
                }

                if (currentInstrument is InstrumentOptionsEnum.ShapeRectangle
                    or InstrumentOptionsEnum.ShapeCircle
                    or InstrumentOptionsEnum.ShapeTriangle)
                {
                    var shape = _shapeManager.CreateShape(currentInstrument, DragStartWorld);
                    if (shape != null)
                    {
                        ComponentManager.GetInstance().AddComponent(shape);
                        _focusManager.UpdateFocus(shape);
                        UpdateManager.GetInstance().TrackUpdate(
                            new CreateUpdate(shape.Id, ComponentSerializer.Serialize(shape)));
                        MenuManager.GetInstance().CurrentInstrument = InstrumentOptionsEnum.Cursor;
                    }
                    return;
                }

                if (currentInstrument is InstrumentOptionsEnum.Brush
                    or InstrumentOptionsEnum.Pencil
                    or InstrumentOptionsEnum.Eraser)
                {
                    _toolManager.OnMousePressed(DragStartWorld);
                    if (_toolManager.ActiveTool is EraserTool eraser)
                    {
                        var components = ComponentManager.GetInstance().GetComponentsForLogic();
                        eraser.ProcessEraseCandidates(components);

                        foreach (var c in eraser.EraseCandidates)
                        {
                            ComponentManager.GetInstance().RemoveComponent(c);
                            if (c is InteractiveComponentBase interactive)
                            {
                                UpdateManager.GetInstance().TrackUpdate(
                                    new RemoveUpdate(c.Id, ComponentSerializer.Serialize(interactive)));
                            }
                        }
                    }
                    else
                    {
                        var created = _toolManager.ActiveTool?.CreatedComponent;
                        if (created != null)
                        {
                            ComponentManager.GetInstance().AddComponent(created);
                        }
                    }
                    return;
                }

                _focusManager.HandleClick(DragStartWorld);
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
