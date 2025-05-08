using SFML.System;
using SFML.Window;
using WeBoard.Core.Components.Interfaces;


namespace WeBoard.Client.Services.Managers
{
    public class FocusManager
    {
        private static readonly FocusManager Instance = new();
        public  IFocusable? FocusedComponent { get; set; }

        private Vector2f ClickOffset = new(0, 0);
        private ComponentManager _componentManager = ComponentManager.GetInstance();

        public FocusManager()
        {
    
        }

        public void HandleClick(Vector2f point)
        {
            var components = _componentManager.GetComponents(true);

            var pointInt = new Vector2i((int)point.X, (int)point.Y);
            var clickedComponent = components.FirstOrDefault(obj => obj.Intersect(pointInt, out ClickOffset));

            if (clickedComponent is null)
            {
                if (FocusedComponent != null)
                    FocusedComponent.OnLostFocus();
                FocusedComponent = null;
                return;
            }

            
            if(FocusedComponent != null)
            {
                FocusedComponent.OnLostFocus();
            }

            FocusedComponent = clickedComponent;
            FocusedComponent.OnFocus();
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
