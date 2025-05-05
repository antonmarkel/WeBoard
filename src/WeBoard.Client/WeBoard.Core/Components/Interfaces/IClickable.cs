using SFML.System;

namespace WeBoard.Core.Components.Interfaces
{
    public interface IClickable
    {
        public bool Intersect(Vector2i point, out Vector2f offset);
    }
}
