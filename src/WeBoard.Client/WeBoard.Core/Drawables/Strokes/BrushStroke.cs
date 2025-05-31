using System.Collections.Immutable;
using SFML.Graphics;
using SFML.System;
using WeBoard.Core.Components.Base;
using WeBoard.Core.Network.Serializable.Enums;
using WeBoard.Core.Network.Serializable.Interfaces;
using WeBoard.Core.Network.Serializable.Strokes;

namespace WeBoard.Core.Drawables.Strokes
{
    public class BrushStroke : InteractiveComponentBase
    {
        private CircleShape _circleShape;
        private List<Vector2f> _points = new();
        private Vector2f _size = new(1, 1);
        private readonly RectangleShape _focusShape = new();
        private Color _color;
        private float _radius;
        protected override Shape Shape => _focusShape;

        public BrushStroke(Color color, float radius)
        {
            _color = color;
            _radius = radius;

            _focusShape.FillColor = Color.Transparent;
            _focusShape.OutlineThickness = 0;
            _focusShape.OutlineColor = new Color(0, 0, 0, 0);
            _circleShape = new CircleShape(_radius / 2f)
            {
                FillColor = _color,
                OutlineThickness = _radius / 3f,
                OutlineColor = new Color(_color.R, _color.G, _color.B, (byte)(_color.A / 3))
            };
        }

        public void AddPoint(Vector2f position)
        {
            _points.Add(position);
            UpdateBounds();
        }

        private void UpdateBounds()
        {
            if (_points.Count == 0)
                return;

            float left = float.MaxValue;
            float top = float.MaxValue;
            float right = float.MinValue;
            float bottom = float.MinValue;

            for (int i = 0; i < _points.Count; i++)
            {
                left = MathF.Min(left, _points[i].X);
                top = MathF.Min(top, _points[i].Y);
                right = MathF.Max(right, _points[i].X);
                bottom = MathF.Max(bottom, _points[i].Y);
            }

            _size = new Vector2f(right - left + 2 * _radius, bottom - top + 2 * _radius);
            Position = new Vector2f((left + right) / 2f, (top + bottom) / 2f);

            _focusShape.Size = _size;
            _focusShape.Origin = _size / 2f;
            _focusShape.Position = Position;
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            foreach (var dot in _points)
            {
                _circleShape.Position = dot;
                target.Draw(_circleShape, states);
            }

            base.Draw(target, states);
        }

        public override void Drag(Vector2f offset)
        {
            base.Drag(offset);

            for (int i = 0; i < _points.Count; i++)
            {
                _points[i] += offset;
            }

            UpdateBounds();
        }

        public override void OnFocus()
        {
            IsInFocus = true;
            UpdateFocusShape();
        }

        public override Vector2f GetSize() => _size;
        public override void SetSize(Vector2f size) => _size = size;

        protected override void UpdateHandles() { }
        public override void FromSerializable(IBinarySerializable serializable)
        {
            if (serializable is StrokeSerializable strokeSerializable)
            {
                ZIndex = strokeSerializable.ZIndex;
                Id = strokeSerializable.Id;
                Position = strokeSerializable.Position;
                SetSize(strokeSerializable.Size);
                _color = strokeSerializable.Color;
                _radius = strokeSerializable.Radius;

                _circleShape = new CircleShape(_radius / 2f)
                {
                    FillColor = _color,
                    OutlineThickness = _radius / 3f,
                    OutlineColor = new Color(_color.R, _color.G, _color.B, (byte)(_color.A / 3))
                };

                foreach (var dot in strokeSerializable.Dots)
                {
                    AddPoint(dot);
                }
            }
        }

        public override IBinarySerializable ToSerializable()
        {
            return new StrokeSerializable((byte)SerializableTypeIdEnum.Brush)
            {
                ZIndex = ZIndex,
                Id = Id,
                Position = Position,
                Size = GetSize(),
                Color = _color,
                Dots = _points.ToImmutableList(),
                Radius = _radius
            };
        }
    }
}
