using SFML.Graphics;
using SFML.System;
using WeBoard.Core.Components.Base;
using WeBoard.Core.Components.Interfaces;

namespace WeBoard.Core.Components.Handlers
{
    public class RotateHandler : IComponent, IDraggable
    {
        private readonly InteractiveComponentBase _target;
        private readonly Sprite _sprite;
        private readonly Texture _texture;

        private Vector2f _center;
        private float _startAngle;
        private Vector2f _startVector;
        private bool _isDragging = false;

        private const float OffsetFromCorner = 25f;

        public bool IsInFocus { get; set; }
        public int ZIndex { get; set; }

        public Vector2f Position
        {
            get => _sprite.Position;
            set => _sprite.Position = value;
        }

        public RotateHandler(InteractiveComponentBase target)
        {
            _target = target;
            _texture = new Texture("Resources/Handlers/Arrow.png");
            _sprite = new Sprite(_texture)
            {
                Scale = new Vector2f(0.08f, 0.08f),
                Origin = new Vector2f(_texture.Size.X / 2f, _texture.Size.Y / 2f)
            };
        }

        public void UpdatePosition(FloatRect bounds)
        {
            _center = _target.Position;

            Vector2f localOffset = new Vector2f(
                _target.GetSize().X / 2f + OffsetFromCorner,
                -_target.GetSize().Y / 2f - OffsetFromCorner
            );

            float angleRad = _target.Rotation * MathF.PI / 180f;
            float cos = MathF.Cos(angleRad);
            float sin = MathF.Sin(angleRad);

            Vector2f rotatedOffset = new Vector2f(
                localOffset.X * cos - localOffset.Y * sin,
                localOffset.X * sin + localOffset.Y * cos
            );

            _sprite.Position = _center + rotatedOffset;
            _sprite.Rotation = _target.Rotation;
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            target.Draw(_sprite, states);
        }

        public FloatRect GetGlobalBounds() => _sprite.GetGlobalBounds();
        public float GetTotalArea() => _sprite.GetGlobalBounds().Width * _sprite.GetGlobalBounds().Height;

        public void Drag(Vector2f offset)
        {
            if (!_isDragging)
            {
                _center = _target.Position;
                var mouseStartPos = _sprite.Position + offset;
                _startVector = mouseStartPos - _center;
                _startAngle = _target.Rotation;
                _isDragging = true;
                return;
            }

            var currentMousePos = _sprite.Position + offset;
            var currentVector = currentMousePos - _center;

            if (currentVector == default || _startVector == default)
                return;

            float startAngleRad = MathF.Atan2(_startVector.Y, _startVector.X);
            float currentAngleRad = MathF.Atan2(currentVector.Y, currentVector.X);
            float deltaAngle = (currentAngleRad - startAngleRad) * 180f / MathF.PI;

            _target.SetRotation(_startAngle + deltaAngle);
            UpdatePosition(_target.GetGlobalBounds());
        }

        public void ResetDrag()
        {
            _isDragging = false;
        }

        public bool Intersect(Vector2i point, out Vector2f offset)
        {
            var bounds = GetGlobalBounds();
            offset = bounds.Position - new Vector2f(point.X, point.Y);
            return bounds.Contains(point.X, point.Y);
        }

        public void OnFocus() => IsInFocus = true;
        public void OnLostFocus() => IsInFocus = false;
        public void OnMouseLeave() { }
        public void OnMouseOver() { }
    }
}
