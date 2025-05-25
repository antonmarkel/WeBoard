using SFML.Graphics;
using SFML.System;
using WeBoard.Core.Components.Shapes.Base;

namespace WeBoard.Core.Components.Shapes
{
    public class Triangle : ShapeBase
    {
        private ConvexShape _triangleShape;
        private Vector2f _size;

        protected override Shape Shape => _triangleShape;

        public Triangle(Vector2f size, Vector2f position)
        {
            _triangleShape = new ConvexShape(3)
            {
                Origin = size / 2f
            };
            _size = size;
            UpdateTrianglePoints(size);
            Position = position;
        }

        public override Vector2f GetSize() => _size;

        public override void SetSize(Vector2f size)
        {
            _size = new Vector2f(
                Math.Max(size.X, MinWidth),
                Math.Max(size.Y, MinHeight)
            );
            _triangleShape.Origin = _size / 2f;
            UpdateTrianglePoints(_size);
            UpdateHandles();
            UpdateFocusShape();

            base.SetSize(size);
        }

        private void UpdateTrianglePoints(Vector2f size)
        {
            _triangleShape.SetPoint(0, new Vector2f(size.X / 2f, 0));
            _triangleShape.SetPoint(1, new Vector2f(size.X, size.Y));
            _triangleShape.SetPoint(2, new Vector2f(0, size.Y));
        }
    }
}