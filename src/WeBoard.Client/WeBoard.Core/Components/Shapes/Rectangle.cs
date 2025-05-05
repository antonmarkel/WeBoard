using SFML.Graphics;
using SFML.System;
using WeBoard.Core.Components.Interfaces;

namespace WeBoard.Core.Components.Shapes
{
    public class Rectangle : IComponent,IDraggable,IClickable,Drawable
    {
        private readonly RectangleShape _shape;
        private readonly RectangleShape _focusRectangle;

        public ICollection<FloatRect> Collisions { get; set; } = [];
        public Rectangle(RectangleShape shape)
        {
            _shape = shape;
            _focusRectangle = new RectangleShape();
            AdjustFocusRectangleSize();

        }

        public bool IsInFocus { get; set; } = false;
        public void OnFocus()
        {
            AdjustFocusRectangleSize();
            IsInFocus = true;
        }

        public void OnLostFocus()
        {
            IsInFocus = false;
        }
        public void Drag(Vector2f offset)
        {
            _shape.Position += offset;
            AdjustFocusRectangleSize();
        }
        public void DragTo(Vector2f coords)
        {
            _shape.Position = coords;
            AdjustFocusRectangleSize();
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            _shape.Draw(target, states);
            if(IsInFocus)_focusRectangle.Draw(target, states);
        }

        private void AdjustFocusRectangleSize()
        {
            _focusRectangle.FillColor = Color.Transparent;
            _focusRectangle.Position = _shape.Position;
            _focusRectangle.OutlineThickness = 4;
            _focusRectangle.OutlineColor = Color.Black;
            _focusRectangle.Size = _shape.Size;
        }

        public bool Intersect(Vector2i point,out Vector2f offset)
        {
            FloatRect bounds = _shape.GetGlobalBounds();
            offset = new Vector2f(point.X, point.Y) - bounds.Position; 

            return bounds.Contains(point.X, point.Y);
        }
    }
}
