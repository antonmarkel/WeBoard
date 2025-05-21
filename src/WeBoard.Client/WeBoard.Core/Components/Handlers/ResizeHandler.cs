using System.Runtime.CompilerServices;
using SFML.Graphics;
using SFML.System;
using WeBoard.Core.Components.Base;
using WeBoard.Core.Components.Interfaces;
using WeBoard.Core.Enums;
using WeBoard.Core.Updates.Interactive;

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
        public IComponent? Parent { get; set; }

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
            if (_target is InteractiveComponentBase component)
            {
                float angleRad = -component.Rotation * MathF.PI / 180f;
                float cos = MathF.Cos(angleRad);
                float sin = MathF.Sin(angleRad);

                Vector2f localOffset = new Vector2f(
                    offset.X * cos - offset.Y * sin,
                    offset.X * sin + offset.Y * cos
                );

                _target.Resize(localOffset, _direction);
            }
        }


        public void UpdatePosition(Vector2f center, Vector2f size, float rotationDegrees)
        {
            float sizeFactor = Math.Min(size.X, size.Y) * 0.05f;
            float radius = Math.Clamp(sizeFactor, 4f, 15f);
            _shape.Radius = radius;
            _shape.Origin = new Vector2f(radius, radius);

            Vector2f localOffset = _direction switch
            {
                ResizeDirectionEnum.TopLeft => new Vector2f(-size.X / 2f, -size.Y / 2f),
                ResizeDirectionEnum.TopRight => new Vector2f(size.X / 2f, -size.Y / 2f),
                ResizeDirectionEnum.BottomLeft => new Vector2f(-size.X / 2f, size.Y / 2f),
                ResizeDirectionEnum.BottomRight => new Vector2f(size.X / 2f, size.Y / 2f),
                _ => new Vector2f(0, 0),
            };

            float angleRad = rotationDegrees * MathF.PI / 180f;
            float cos = MathF.Cos(angleRad);
            float sin = MathF.Sin(angleRad);

            Vector2f rotatedOffset = new Vector2f(
                localOffset.X * cos - localOffset.Y * sin,
                localOffset.X * sin + localOffset.Y * cos
            );

            _shape.Position = center + rotatedOffset;
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
            var wasInFocus = IsInFocus;
            IsInFocus = false;
            if(wasInFocus)
                _target.OnFinishResizing();
        }

        public void OnMouseLeave() { }
        public void OnMouseOver() { }
    }
}
