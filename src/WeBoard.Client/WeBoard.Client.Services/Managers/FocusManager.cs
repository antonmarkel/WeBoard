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
        private IDraggable? DraggingComponent { get; set; }

        public FocusManager()
        {

        }

        public void HandleClick(Vector2f point)
        {
            var components = _componentManager.GetComponentsForLogic();

            var pointInt = new Vector2i((int)point.X, (int)point.Y);

            foreach (var comp in components)
            {
                if (comp is InteractiveComponentBase interactive)
                {
                    foreach (var handle in interactive.GetResizeHandles())
                    {
                        if (handle.Intersect(pointInt, out _))
                        {
                            DraggingComponent = handle;
                            interactive.OnFocus();
                            return;
                        }
                    }

                    var rotateHandle = interactive.GetRotateHandle();
                    if (rotateHandle != null && rotateHandle.Intersect(pointInt, out _))
                    {
                        DraggingComponent = rotateHandle;
                        interactive.OnFocus();
                        return;
                    }
                }
            }

            DraggingComponent = null;

            var clickedComponent = components.FirstOrDefault(obj => obj.Intersect(pointInt, out _));

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

        public void HandleDrag(Vector2f offset)
        {
            if (FocusedComponent is not IDraggable)
                return;
            
            if (DraggingComponent is not null)
            {
                DraggingComponent.Drag(offset);
                return;
            }
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

        public void ClearDragging()
        {
            DraggingComponent = null;
        }


        public static FocusManager GetInstance()
        {
            return Instance;
        }
    }
}
