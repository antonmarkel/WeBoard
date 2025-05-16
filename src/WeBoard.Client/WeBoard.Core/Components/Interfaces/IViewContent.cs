using SFML.Graphics;
using SFML.System;

namespace WeBoard.Core.Components.Interfaces
{
    public interface IViewContent : Drawable
    {
        public Vector2f Position { get; set; }
        public Vector2f Size { get; set; }
        public float Rotation { get; set; }
        public abstract void Draw(RenderTarget target, RenderStates states);
    }
}
