using SFML.Graphics;
using SFML.System;
using WeBoard.Core.Components.Base;

namespace WeBoard.Core.Components.Shapes
{
    public class Rectangle : InteractiveComponentBase
    {
        private RectangleShape _rectangleShape;

        public Rectangle(RectangleShape rectangleShape, Vector2f position) : base()
        {
            _rectangleShape = rectangleShape;
            Position = position;
        }

        public override Vector2f Position { 
            get => _rectangleShape.Position;
            set
             {
                base.Position = value;
                _rectangleShape.Position = value; 
             } 
        }

        public override FloatRect GetGlobalBounds()
        {
            return _rectangleShape.GetGlobalBounds();
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            base.Draw(target, states);
            _rectangleShape.Draw(target, states);
        }

        public override void Drag(Vector2f offset)
        {
            Position += offset;
        }
    }
}
