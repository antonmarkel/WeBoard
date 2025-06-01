using SFML.System;

namespace WeBoard.Core.Components.Interfaces
{
    public interface IDraggable : IFocusable
    {
        public void OnStartDragging() { }

        public void Drag(Vector2f offset);

        public void OnStopDragging() { }
    }
}
