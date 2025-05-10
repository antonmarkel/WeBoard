using SFML.Graphics;
using SFML.System;
using WeBoard.Core.Components.Interfaces;
using WeBoard.Core.Enums;

namespace WeBoard.Core.Components.Handlers
{
    public class ResizeHandler : IComponent, IDraggable
    {
        private CircleShape _shape;
        private readonly IResizable _target;
        private readonly ResizeDirectionEnum _direction;
        public int ZIndex { get; set; }
        public bool IsInFocus { get; set; }
        public Vector2f Position
        {
            get => _shape.Position;
            set => _shape.Position = value;
        }

        public ResizeHandler(IResizable target, ResizeDirectionEnum direction)
        {
            _target = target;
            _direction = direction;
            _shape = new CircleShape(5f)
            {
                FillColor = Color.White,
                OutlineColor = Color.Black,
                OutlineThickness = 1f,
            };
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            target.Draw(_shape, states);
        }

        public FloatRect GetGlobalBounds() => _shape.GetGlobalBounds();
        public float GetTotalArea() => _shape.GetGlobalBounds().Width * _shape.GetGlobalBounds().Height;

        public void Drag(Vector2f offset)
        {
            _target.Resize(offset, _direction);
        }

        public void UpdatePosition(FloatRect bounds)
        {
            float sizeFactor = Math.Min(bounds.Width, bounds.Height) * 0.05f;
            float radius = Math.Clamp(sizeFactor, 4f, 15f);
            _shape.Radius = radius;
            _shape.Origin = new Vector2f(radius, radius);

            Vector2f pos = _direction switch
            {
                ResizeDirectionEnum.TopLeft => new(bounds.Left, bounds.Top),
                ResizeDirectionEnum.TopRight => new(bounds.Left + bounds.Width, bounds.Top),
                ResizeDirectionEnum.BottomLeft => new(bounds.Left, bounds.Top + bounds.Height),
                ResizeDirectionEnum.BottomRight => new(bounds.Left + bounds.Width, bounds.Top + bounds.Height),
                _ => bounds.Position,
            };

            _shape.Position = pos;
        }

        public bool Intersect(Vector2i point, out Vector2f offset)
        {
            var bounds = GetGlobalBounds();
            offset = bounds.Position - new Vector2f(point.X, point.Y);
            return bounds.Contains(point.X, point.Y);
        }

        public void OnFocus()
        {
            IsInFocus = true;
        }

        public void OnLostFocus()
        {
            IsInFocus = false;
        }

        public void OnMouseLeave() { }
        public void OnMouseOver() { }
    }
}
