using SFML.Graphics;
using SFML.System;
using WeBoard.Core.Components.Tools.Base;
using WeBoard.Core.Drawables.Strokes;

namespace WeBoard.Core.Components.Tools
{
    public class BrushTool : ToolBase
    {
        private BrushStroke? _currentStroke;
        private bool _isDrawing = false;

        public override void OnMousePressed(Vector2f worldPos)
        {
            _currentStroke = new BrushStroke();
            _currentStroke.AddPoint(worldPos, ToolColor, ToolSize);
            CreatedComponent = _currentStroke;
            _isDrawing = true;
        }

        public override void OnMouseMoved(Vector2f worldPos)
        {
            if (_isDrawing && _currentStroke != null)
                _currentStroke.AddPoint(worldPos, ToolColor, ToolSize);
        }

        public override void OnMouseReleased(Vector2f worldPos)
        {
            _isDrawing = false;
            _currentStroke = null;
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            if (_currentStroke != null)
                _currentStroke.Draw(target, states);
        }
    }
}