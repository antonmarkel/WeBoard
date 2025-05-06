using SFML.System;
using SFML.Window;
using WeBoard.Client.Core.Engine;
using WeBoard.Core.Components.Interfaces;


namespace WeBoard.Client.Services.Managers
{
    public class FocusManager
    {
        private static readonly FocusManager Instance = new();
        public  IFocusable? FocusedComponent { get; set; }

        private Vector2f ClickOffset = new(0, 0);
        private BoardGlobal _global = BoardGlobal.GetInstance();

        public FocusManager()
        {
    
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

            if(focusedComponent != null)
            {
                if(FocusedComponent != null)
                {
                    FocusedComponent.OnLostFocus();
                }

                FocusedComponent = focusedComponent;
                FocusedComponent.OnFocus();

                return;
            }

            FocusedComponent = focusedComponent;
         

        }

        public void HandleDrag(Vector2f offset)
        {
            if (FocusedComponent is not IDraggable)
                return;

            ((IDraggable)FocusedComponent).Drag(offset);

        }

        public void ClearFocus()
        {
            if (FocusedComponent != null)
            {
                FocusedComponent.OnLostFocus();
            }
            FocusedComponent = null;
        }

        public static FocusManager GetInstance()
        {
            return Instance;
        }
    }
}
