using SFML.Graphics;
using SFML.System;
using WeBoard.Core.Components.Base;

namespace WeBoard.Core.Drawables.Strokes
{
    public class BrushStroke : InteractiveComponentBase
    {
        private readonly List<CircleShape> _dots = new();
        private Vector2f _size = new(1, 1);
        private readonly RectangleShape _focusShape = new();

        protected override Shape Shape => _focusShape;

        public BrushStroke()
        {
            _focusShape.FillColor = Color.Transparent;
            _focusShape.OutlineThickness = 0;
            _focusShape.OutlineColor = new Color(0, 0, 0, 0);
        }

        public void AddPoint(Vector2f position, Color color, float radius)
        {
            var circle = new CircleShape(radius / 2f)
            {
                FillColor = color,
                Position = position - new Vector2f(radius / 2f, radius / 2f),
                OutlineThickness = radius / 3f,
                OutlineColor = new Color(color.R, color.G, color.B, (byte)(color.A / 3))
            };

            _dots.Add(circle);
            UpdateBounds();
        }

        private void UpdateBounds()
        {
            if (_dots.Count == 0)
                return;

            var bounds = _dots[0].GetGlobalBounds();
            float left = bounds.Left;
            float top = bounds.Top;
            float right = bounds.Left + bounds.Width;
            float bottom = bounds.Top + bounds.Height;

            for (int i = 1; i < _dots.Count; i++)
            {
                var b = _dots[i].GetGlobalBounds();
                left = MathF.Min(left, b.Left);
                top = MathF.Min(top, b.Top);
                right = MathF.Max(right, b.Left + b.Width);
                bottom = MathF.Max(bottom, b.Top + b.Height);
            }

            _size = new Vector2f(right - left, bottom - top);
            Position = new Vector2f((left + right) / 2f, (top + bottom) / 2f);

            _focusShape.Size = _size;
            _focusShape.Origin = _size / 2f;
            _focusShape.Position = Position;
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            foreach (var dot in _dots)
                target.Draw(dot, states);

            base.Draw(target, states);
        }

        public override void Drag(Vector2f offset)
        {
            base.Drag(offset);

            foreach (var dot in _dots)
            {
                dot.Position += offset;
            }

            UpdateBounds();
        }

        public override void OnFocus()
        {
            IsInFocus = true;
            UpdateFocusShape();
        }

        public override Vector2f GetSize() => _size;
        protected override void UpdateHandles() { }
    }
}
