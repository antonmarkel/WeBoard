using SFML.System;
using WeBoard.Core.Components.Interfaces;

namespace WeBoard.Client.Services.Managers
{
    public class FocusManager
    {
        private static readonly FocusManager Instance = new();
        public IFocusable? FocusedComponent { get; set; }
        public IMouseDetective? UnderMouse { get; set; }
        public IScrollable? Scrollable { get; set; }
        private ComponentManager _componentManager = ComponentManager.GetInstance();

        public FocusManager()
        {

        }

        public void HandleClick(Vector2f worldPoint, Vector2i screenPoint)
        {

            var clickedComponent = _componentManager.GetByPoints(worldPoint, screenPoint);

            if (clickedComponent is null || clickedComponent is not IFocusable)
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

            FocusedComponent = (IFocusable)clickedComponent;
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
