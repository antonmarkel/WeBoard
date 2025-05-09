using SFML.System;

namespace WeBoard.Core.Components.Interfaces
{
    public interface IDraggable
    {
        public void Drag(Vector2f offset);
    }
}
