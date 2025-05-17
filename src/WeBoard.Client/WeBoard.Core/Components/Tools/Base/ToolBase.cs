using SFML.System;
using SFML.Graphics;

namespace WeBoard.Core.Components.Tools.Base
{
    public abstract class ToolBase
    {
        public abstract void OnMousePressed(Vector2f worldPos);
        public abstract void OnMouseReleased(Vector2f worldPos);
        public abstract void OnMouseMoved(Vector2f worldPos);
        public virtual void Draw(RenderTarget target, RenderStates states) { }
    }
}