using SFML.Graphics;
using SFML.System;
using WeBoard.Core.Components.Base;

namespace WeBoard.Core.Components.Shapes
{
    public class Ellipse : InteractiveComponentBase
    {
        private ConvexShape _ellipseShape;
        private Vector2f _size;

        public Ellipse(Vector2f size, Vector2f position) : base()
        {
            _ellipseShape = new ConvexShape(60);
            _ellipseShape.FillColor = Color.White;

            _size = size;
            SetSize(size);
            Position = position;

            UpdateHandles();
        }

        public override Vector2f Position
        {
            get => _ellipseShape.Position;
            set
            {
                base.Position = value;
                _ellipseShape.Position = value;
            }
        }

        public Color FillColor
        {
            get => _ellipseShape.FillColor;
            set => _ellipseShape.FillColor = value;
        }

        public override FloatRect GetGlobalBounds()
        {
            return _ellipseShape.GetGlobalBounds();
        }

        public override Vector2f GetSize() => _size;

        public override void SetSize(Vector2f size)
        {
            _size = new Vector2f(
                Math.Max(size.X, MinWidth),
                Math.Max(size.Y, MinHeight)
            );

            float a = _size.X / 2f;
            float b = _size.Y / 2f;

            for (uint i = 0; i < _ellipseShape.GetPointCount(); i++)
            {
                float angle = i * 2 * MathF.PI / _ellipseShape.GetPointCount();
                float x = MathF.Cos(angle) * a;
                float y = MathF.Sin(angle) * b;
                _ellipseShape.SetPoint(i, new Vector2f(x + a, y + b));
            }
        }

        public override void Drag(Vector2f offset)
        {
            Position += offset;
            base.Drag(offset);
            UpdateHandles();
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            target.Draw(_ellipseShape, states);
            base.Draw(target, states);
        }
    }
}
