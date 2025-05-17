using SFML.Graphics;
using SFML.System;
using WeBoard.Core.Components.Tools.Base;

namespace WeBoard.Core.Components.Tools
{
    public class BrushTool : ToolBase
    {
        private readonly List<List<Vertex>> _lines = new();
        private List<Vertex> _currentLine = new();
        private bool _isDrawing = false;

        public override void OnMousePressed(Vector2f worldPos)
        {
            _isDrawing = true;
            _currentLine = new List<Vertex>();
            _currentLine.Add(new Vertex(worldPos, Color.Black));
            _lines.Add(_currentLine);
        }

        public override void OnMouseReleased(Vector2f worldPos)
        {
            _isDrawing = false;
        }

        public override void OnMouseMoved(Vector2f worldPos)
        {
            if (_isDrawing)
                _currentLine.Add(new Vertex(worldPos, Color.Black));
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            foreach (var line in _lines)
            {
                if (line.Count > 1)
                    target.Draw(line.ToArray(), PrimitiveType.LineStrip);
            }
        }
    }
}