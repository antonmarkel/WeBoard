using SFML.Graphics;
using SFML.System;
using WeBoard.Core.Components.Base;
using WeBoard.Core.Components.Tools.Base;

namespace WeBoard.Core.Components.Tools
{
    public class EraserTool : ToolBase
    {
        private bool _isErasing = false;
        private Vector2f _currentPosition;
        private float _eraserRadius => ToolSize;

        private readonly HashSet<ComponentBase> _eraseCandidates = new();

        public IEnumerable<ComponentBase> EraseCandidates => _eraseCandidates;

        public override void OnMousePressed(Vector2f worldPos)
        {
            _isErasing = true;
            _currentPosition = worldPos;
            _eraseCandidates.Clear();
        }

        public override void OnMouseMoved(Vector2f worldPos)
        {
            if (!_isErasing)
                return;

            _currentPosition = worldPos;
        }

        public override void OnMouseReleased(Vector2f worldPos)
        {
            _isErasing = false;
        }
        public void ProcessEraseCandidates(IEnumerable<ComponentBase> components)
        {
            foreach (var component in components)
            {
                if (component == null)
                    continue;

                if (CreatedComponent == component)
                    continue;

                var bounds = component.GetGlobalBounds();

                if (CircleIntersectsRect(_currentPosition, _eraserRadius, bounds))
                {
                    _eraseCandidates.Add(component);
                }
            }
        }
        private bool CircleIntersectsRect(Vector2f circleCenter, float radius, FloatRect rect)
        {
            float closestX = Clamp(circleCenter.X, rect.Left, rect.Left + rect.Width);
            float closestY = Clamp(circleCenter.Y, rect.Top, rect.Top + rect.Height);

            float dx = circleCenter.X - closestX;
            float dy = circleCenter.Y - closestY;

            return (dx * dx + dy * dy) <= (radius * radius);
        }

        private float Clamp(float val, float min, float max)
        {
            if (val < min) return min;
            if (val > max) return max;
            return val;
        }
    }
}
