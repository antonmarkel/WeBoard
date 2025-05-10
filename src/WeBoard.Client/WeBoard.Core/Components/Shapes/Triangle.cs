using SFML.Graphics;
using SFML.System;
using WeBoard.Core.Components.Base;

namespace WeBoard.Core.Components.Shapes
{
    public class Triangle : InteractiveComponentBase
    {
        private ConvexShape _triangleShape;
        private Vector2f _size;

        public Triangle(Vector2f size, Vector2f position) : base()
        {
            _triangleShape = new ConvexShape(3);
            _size = size;
            Position = position;

            UpdateShape();
            UpdateHandles();
        }

        public override Vector2f Position
        {
            get => _triangleShape.Position;
            set
            {
                base.Position = value;
                _triangleShape.Position = value;
            }
        }

        public Color FillColor
        {
            get => _triangleShape.FillColor;
            set => _triangleShape.FillColor = value;
        }

        public override FloatRect GetGlobalBounds()
        {
            return _triangleShape.GetGlobalBounds();
        }

        public override Vector2f GetSize() => _size;

        public override void SetSize(Vector2f size)
        {
            _size = new Vector2f(
                Math.Max(size.X, MinWidth),
                Math.Max(size.Y, MinHeight)
            );

            UpdateShape();
        }

        public override void Drag(Vector2f offset)
        {
            Position += offset;
            base.Drag(offset);
            UpdateHandles();
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            _triangleShape.Draw(target, states);
            base.Draw(target, states);      
        }

        private void UpdateShape()
        {
            _triangleShape.SetPoint(0, new Vector2f(_size.X / 2f, 0));
            _triangleShape.SetPoint(1, new Vector2f(_size.X, _size.Y));
            _triangleShape.SetPoint(2, new Vector2f(0, _size.Y));
        }
    }
}
