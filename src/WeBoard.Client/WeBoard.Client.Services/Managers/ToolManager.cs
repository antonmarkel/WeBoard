using SFML.System;
using SFML.Graphics;
using WeBoard.Core.Components.Tools.Base;

namespace WeBoard.Client.Services.Managers
{
    public class ToolManager
    {
        private static readonly ToolManager Instance = new();
        public static ToolManager GetInstance() => Instance;

        public ToolBase? ActiveTool { get; set; }

        public void OnMousePressed(Vector2f pos)
        {
            ActiveTool?.OnMousePressed(pos);
        }

        public void OnMouseReleased(Vector2f pos)
        {
            ActiveTool?.OnMouseReleased(pos);
        }

        public void OnMouseMoved(Vector2f pos)
        {
            ActiveTool?.OnMouseMoved(pos);
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            ActiveTool?.Draw(target, states);
        }
    }
}
