using SFML.Graphics;
using SFML.System;
using WeBoard.Core.Components.Base;

namespace WeBoard.Core.Components.Shapes
{
    public class Rectangle : InteractiveComponentBase
    {
        private readonly RectangleShape _rectangleShape;
        protected override Shape Shape => _rectangleShape;

        public Rectangle(Vector2f size, Vector2f position)
        {
            _rectangleShape = new RectangleShape(size)
            {
                Origin = size / 2f 
            };
            Position = position;
        }

        public override Vector2f GetSize() => _rectangleShape.Size;

        public override void SetSize(Vector2f size)
        {
            _rectangleShape.Size = new Vector2f(
                Math.Max(size.X, MinWidth),
                Math.Max(size.Y, MinHeight)
            );
            _rectangleShape.Origin = _rectangleShape.Size / 2f;
            UpdateHandles();
            UpdateFocusShape();
        }
    }
}