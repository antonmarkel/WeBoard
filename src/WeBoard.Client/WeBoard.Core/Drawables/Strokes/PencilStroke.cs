using SFML.Graphics;
using SFML.System;
using WeBoard.Core.Components.Base;
using WeBoard.Core.Network.Serializable.Enums;
using WeBoard.Core.Network.Serializable.Interfaces;
using WeBoard.Core.Network.Serializable.Strokes;

namespace WeBoard.Core.Drawables.Strokes
{
    public class PencilStroke : InteractiveComponentBase
    {
        private List<Vector2f> _points = new();
        private Color _color;
        private VertexArray _vertexArray = new(PrimitiveType.LineStrip);
        private Vector2f _size = new(1, 1);
        private readonly RectangleShape _focusShape = new();

        protected override Shape Shape => _focusShape;

        public PencilStroke(Color color)
        {
            _focusShape.FillColor = Color.Transparent;
            _focusShape.OutlineThickness = 0;
            _focusShape.OutlineColor = new Color(0, 0, 0, 0);

            _color = color;
        }

        public void AddPoint(Vector2f position)
        {
            _points.Add(position);
            _vertexArray.Append(new Vertex(position, _color));
            UpdateBounds();
        }

        private void UpdateBounds()
        {
            if (_points.Count == 0) return;

            float left = _points[0].X, right = _points[0].X;
            float top = _points[0].Y, bottom = _points[0].Y;

            foreach (var p in _points)
            {
                left = MathF.Min(left, p.X);
                right = MathF.Max(right, p.X);
                top = MathF.Min(top, p.Y);
                bottom = MathF.Max(bottom, p.Y);
            }

            _size = new Vector2f(right - left, bottom - top);
            Position = new Vector2f((left + right) / 2f, (top + bottom) / 2f);

            _focusShape.Size = _size;
            _focusShape.Origin = _size / 2f;
            _focusShape.Position = Position;
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            target.Draw(_vertexArray, states);
            base.Draw(target, states);
        }

        public override void Drag(Vector2f offset)
        {
            base.Drag(offset);
            _vertexArray.Clear();
            for (int i = 0; i < _points.Count; i++)
            {
                _points[i] += offset;
                _vertexArray.Append(new Vertex(_points[i], _color));
            }

            UpdateBounds();
        }

        public override void OnFocus()
        {
            IsInFocus = true;
            UpdateFocusShape();
        }

        public override Vector2f GetSize() => _size;
        protected override void UpdateHandles() { }
        public override void SetSize(Vector2f size) { }
        public override void SetRotation(float angle) { }

        public override void FromSerializable(IBinarySerializable serializable)
        {
            if (serializable is StrokeSerializable strokeSerializable)
            {
                ZIndex = strokeSerializable.ZIndex;
                Id = strokeSerializable.Id;
                Position = strokeSerializable.Position;
                _color = strokeSerializable.Color;
                _points = strokeSerializable.Dots.ToList();
                _vertexArray = new(PrimitiveType.LineStrip);
                for (int i = 0; i < strokeSerializable.Dots.Length;i++)
                {
                    AddPoint(strokeSerializable.Dots[i]);
                }

            }
        }

        public override IBinarySerializable ToSerializable()
        {
            return new StrokeSerializable((byte)SerializableTypeIdEnum.Pencil)
            {
                ZIndex = ZIndex,
                Id = Id,
                Position = Position,
                Size = GetSize(),
                Color = _color,
                Dots = _points.ToArray(),
                Radius = 0
            };
        }
    }
}



