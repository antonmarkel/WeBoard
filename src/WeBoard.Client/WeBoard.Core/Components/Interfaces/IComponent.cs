using SFML.Graphics;
using SFML.System;

namespace WeBoard.Core.Components.Interfaces
{
    public interface IComponent : Drawable
    {
        public Vector2f Position { get; set; }
        public int ZIndex { get; set; }
        public FloatRect GetGlobalBounds();
    }
}
