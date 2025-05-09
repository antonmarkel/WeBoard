using SFML.System;
using WeBoard.Core.Components.Base;
using WeBoard.Core.Components.Interfaces;

namespace WeBoard.Client.Services.Managers
{
    public class FocusManager
    {
        private static readonly FocusManager Instance = new();
        public IFocusable? FocusedComponent { get; set; }
        private ComponentManager _componentManager = ComponentManager.GetInstance();

        public FocusManager()
        {

        }

        public void HandleClick(Vector2f point, Vector2i screenPoint)
        {

           
            IEnumerable<MenuComponentBase> menuComponents = MenuManager.GetInstance().GetMenuComponents();
            var clickedComponent = (ComponentBase?)menuComponents.FirstOrDefault(obj => obj.Intersect(screenPoint, out _));
            if(clickedComponent is null)
            {
                var pointInt = new Vector2i((int)point.X, (int)point.Y);
                var components = _componentManager.GetComponentsForLogic();
                clickedComponent = components.FirstOrDefault(obj => obj.Intersect(pointInt, out _));

            }

            if (clickedComponent is null)
            {
                if (FocusedComponent != null)
                    FocusedComponent.OnLostFocus();
                FocusedComponent = null;
                return;
            }

            if (FocusedComponent != null)
            {
                FocusedComponent.OnLostFocus();
            }

            FocusedComponent = clickedComponent;
            FocusedComponent.OnFocus();
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
