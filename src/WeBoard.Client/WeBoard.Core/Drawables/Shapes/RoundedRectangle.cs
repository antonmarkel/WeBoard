using SFML.Graphics;
using SFML.System;

namespace WeBoard.Core.Drawables.Shapes
{
    public class RoundedRectangle : Transformable, Drawable
    {
        private ConvexShape _shape;
        private Vector2f _size;
        private float _cornerRadius;
        private uint _cornerPointCount;

        public RoundedRectangle()
        {
            _shape = new ConvexShape();
            _cornerPointCount = 10;
            Size = new Vector2f(100, 100);
            CornerRadius = 20;
            FillColor = Color.White;
            OutlineColor = Color.White;
            OutlineThickness = 0;
        }

        public Vector2f Size
        {
            get => _size;
            set
            {
                _size = value;
                UpdateShape();
            }
        }

        public float CornerRadius
        {
            get => _cornerRadius;
            set
            {
                _cornerRadius = value;
                UpdateShape();
            }
        }

        public Color FillColor
        {
            get => _shape.FillColor;
            set => _shape.FillColor = value;
        }

        public Color OutlineColor
        {
            get => _shape.OutlineColor;
            set => _shape.OutlineColor = value;
        }

        public float OutlineThickness
        {
            get => _shape.OutlineThickness;
            set => _shape.OutlineThickness = value;
        }

        public uint CornerPointCount
        {
            get => _cornerPointCount;
            set
            {
                _cornerPointCount = value;
                UpdateShape();
            }
        }

        private void UpdateShape()
        {
            List<Vector2f> points = new List<Vector2f>();
            float width = _size.X;
            float height = _size.Y;
            float radius = Math.Min(_cornerRadius, Math.Min(width / 2, height / 2));

            GenerateCornerPoints(points, new Vector2f(radius, radius), 180f, 270f); // Top-Left
            GenerateCornerPoints(points, new Vector2f(width - radius, radius), 270f, 360f); // Top-Right
            GenerateCornerPoints(points, new Vector2f(width - radius, height - radius), 0f, 90f); // Bottom-Right
            GenerateCornerPoints(points, new Vector2f(radius, height - radius), 90f, 180f); // Bottom-Left

            _shape.SetPointCount((uint)points.Count);
            for (int i = 0; i < points.Count; i++)
            {
                _shape.SetPoint((uint)i, points[i]);
            }
        }

        private void GenerateCornerPoints(List<Vector2f> points, Vector2f center, float startAngle, float endAngle)
        {
            float angleStep = (endAngle - startAngle) / _cornerPointCount;
            for (int i = 0; i <= _cornerPointCount; i++)
            {
                float angle = startAngle + i * angleStep;
                float radians = angle * (float)Math.PI / 180f;
                Vector2f offset = new Vector2f(
                    (float)Math.Cos(radians) * _cornerRadius,
                    (float)Math.Sin(radians) * _cornerRadius
                );
                points.Add(center + offset);
            }
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            states.Transform *= Transform;
            target.Draw(_shape, states);
        }
    }
}
