using SFML.Graphics;
using SFML.System;
using WeBoard.Core.Components.Base;

namespace WeBoard.Core.Components.Tools.Base
{
    public abstract class ToolBase
    {
        public virtual ComponentBase? CreatedComponent { get; protected set; }
        public float ToolSize { get; set; } = 1f;
        public Color ToolColor { get; set; } = Color.Black;

        public void SetToolSize(float size)
        {
            ToolSize = MathF.Max(1f, size);
        }

        public void SetToolColor(Color color)
        {
            ToolColor = color;
        }
        public abstract void OnMousePressed(Vector2f worldPos);
        public abstract void OnMouseReleased(Vector2f worldPos);
        public abstract void OnMouseMoved(Vector2f worldPos);
        public virtual void Draw(RenderTarget target, RenderStates states) { }
    }
}