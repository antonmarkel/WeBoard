using SFML.System;
using SFML.Window;
using WeBoard.Client.Core.Engine;
using WeBoard.Core.Components.Interfaces;

namespace WeBoard.Client.Services.Managers
{
    public class FocusManager
    {
        private static readonly FocusManager Instance = new();
        public IFocusable? FocusedComponent { get; set; }

        private Vector2f ClickOffset = new(0, 0);
        private BoardGlobal _global = BoardGlobal.GetInstance();

        public FocusManager()
        {
            _global.RenderWindow!.MouseMoved += HandleMouseMove;
        }

        private void HandleMouseMove(object? sender, MouseMoveEventArgs e)
        {
            if (FocusedComponent is not IDraggable)
                return;

            var coords = _global.RenderWindow!.MapPixelToCoords(new Vector2i(e.X, e.Y), _global.RenderView);
            var draggable = (IDraggable)FocusedComponent;
            draggable.DragTo(coords);
            draggable.Drag(-ClickOffset);
        }


        public void HandleClick(Vector2f point)
        {
            var clickedComponent = _global.RenderObjects.Values.Where(obj => obj is IClickable and IFocusable)
                .FirstOrDefault(obj => ((IClickable)obj).Intersect(new Vector2i((int)point.X, (int)point.Y), out ClickOffset));

            if (clickedComponent is null)
            {
                if (FocusedComponent != null)
                    FocusedComponent.OnLostFocus();
                FocusedComponent = null;
                return;
            }

            var focusedComponent = (IFocusable)clickedComponent;
            if (focusedComponent.IsInFocus)
            {
                focusedComponent.OnLostFocus();
                FocusedComponent = null;
                return;
            }

            focusedComponent.OnFocus();
            FocusedComponent = focusedComponent;




        }

        public void HandleDrag(Vector2f offset)
        {
            if (FocusedComponent is not IDraggable)
                return;

            ((IDraggable)FocusedComponent).Drag(offset);

        }

        public static FocusManager GetInstance()
        {
            return Instance;
        }
    }
}
