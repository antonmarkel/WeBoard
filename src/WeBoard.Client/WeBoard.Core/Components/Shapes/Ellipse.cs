using SFML.Graphics;
using SFML.System;
using WeBoard.Core.Components.Base;
using WeBoard.Core.Components.Shapes.Base;

namespace WeBoard.Core.Components.Shapes
{
    public class Ellipse : ShapeBase
    {
        private ConvexShape _ellipseShape;
        protected override Shape Shape => _ellipseShape;
        private Vector2f _size;

        public Ellipse(Vector2f size, Vector2f position)
        {
            _ellipseShape = new ConvexShape(60)
            {
                Origin = size / 2f 
            };
            _size = size;
            UpdateEllipsePoints(size);
            Position = position;
        }

        public override Vector2f GetSize() => _size;

        public override void SetSize(Vector2f size)
        {
            _size = new Vector2f(
                Math.Max(size.X, MinWidth),
                Math.Max(size.Y, MinHeight)
            );
            _ellipseShape.Origin = _size / 2f;
            UpdateEllipsePoints(_size);
            UpdateHandles();
            UpdateFocusShape();

            base.SetSize(size);
        }

        private void UpdateEllipsePoints(Vector2f size)
        {
            float a = size.X / 2f;
            float b = size.Y / 2f;

            for (uint i = 0; i < _ellipseShape.GetPointCount(); i++)
            {
                float angle = i * 2 * MathF.PI / _ellipseShape.GetPointCount();
                float x = MathF.Cos(angle) * a;
                float y = MathF.Sin(angle) * b;
                _ellipseShape.SetPoint(i, new Vector2f(x + a, y + b));
            }
        }
    }
}
