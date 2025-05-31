using SFML.Graphics;
using SFML.System;
using WeBoard.Core.Components.Interfaces;

namespace WeBoard.Core.Components.Content.Base
{
    public abstract class ContentViewBase : IContentView
    {
        public virtual Vector2f Position { get; set; }
        public virtual Vector2f Size { get; set; }
        public virtual float Rotation { get; set; }
        public abstract void Draw(RenderTarget target, RenderStates states);

    }
}
