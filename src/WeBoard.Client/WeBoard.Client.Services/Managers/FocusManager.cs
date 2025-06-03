using SFML.System;
using WeBoard.Core.Components.Base;
using WeBoard.Core.Components.Interfaces;
using WeBoard.Core.Components.Visuals;
using WeBoard.Core.Updates.Creation;

namespace WeBoard.Client.Services.Managers
{
    public class FocusManager
    {
        private static readonly FocusManager Instance = new();
        public IFocusable? FocusedComponent { get; set; }
        public IFocusable? UnderMouse { get; set; }
        private ComponentManager _componentManager = ComponentManager.GetInstance();
        public IComponent? ActiveHandler { get; set; }
        public FocusManager()
        {

        }

        public void HandleClick(Vector2f point)
        {
            var pointInt = new Vector2i((int)point.X, (int)point.Y);

            if (FocusedComponent != null && FocusedComponent is InteractiveComponentBase interactive)
            {
                ActiveHandler = null;
                foreach (var handle in interactive.GetResizeHandles())
                {
                    if (handle.Intersect(pointInt, out _))
                    {
                        ActiveHandler = handle;
                        handle.OnFocus();
                        interactive.OnFocus();
                    }
                    else
                    {
                        handle.OnLostFocus();
                    }
                }

                if (ActiveHandler != null)
                    return;

                var rotateHandle = interactive.GetRotateHandle();
                if (rotateHandle != null && rotateHandle.Intersect(pointInt, out _))
                {
                    ActiveHandler = rotateHandle;
                    rotateHandle.OnFocus();
                    interactive.OnFocus();
                }
                else
                {
                    rotateHandle?.OnLostFocus();
                }

                if (ActiveHandler != null)
                    return;

            }

            var clickedComponent = _componentManager.GetByScreenPoint(pointInt, out _);

            ActiveHandler = null;

            UpdateFocus(clickedComponent);
        }

        public void UpdateFocus(ComponentBase? clickedComponent)
        {
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

        public static FocusManager GetInstance()
        {
            return Instance;
        }
    }
}
