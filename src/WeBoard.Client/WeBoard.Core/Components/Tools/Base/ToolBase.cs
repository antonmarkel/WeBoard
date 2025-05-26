using SFML.System;
using SFML.Graphics;

namespace WeBoard.Core.Components.Tools.Base
{
    public abstract class ToolBase
    {
        public float BrushSize { get; private set; } = 1f;
        public Color BrushColor { get; private set; } = Color.Black;

        public void SetBrushSize(float size)
        {
            BrushSize = MathF.Max(1f, size);
        }

        public void SetBrushColor(Color color)
        {
            BrushColor = color;
        }
        public abstract void OnMousePressed(Vector2f worldPos);
        public abstract void OnMouseReleased(Vector2f worldPos);
        public abstract void OnMouseMoved(Vector2f worldPos);
        public virtual void Draw(RenderTarget target, RenderStates states) { }
    }
}