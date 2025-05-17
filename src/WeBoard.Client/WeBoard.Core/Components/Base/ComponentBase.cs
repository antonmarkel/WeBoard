using SFML.Graphics;
using SFML.System;
using WeBoard.Core.Components.Interfaces;

namespace WeBoard.Core.Components.Base
{
    public abstract class ComponentBase : IComponent, IFocusable
    {
        private RectangleShape _focusShape;
        protected abstract Shape Shape { get; }
        public bool IsInFocus { get; set; }
        private int _zIndex;
        public int ZIndex
        {
            get => _zIndex;
            set
            {
                _zIndex = value;
                ZIndexChanged?.Invoke(this);
            }
        }
        public event Action<ComponentBase> ZIndexChanged;
        public virtual Vector2f Position { get => Shape.Position; set => Shape.Position = value; }

        public ComponentBase()
        {
            _focusShape = new RectangleShape()
            {
                FillColor = Color.Transparent,
                OutlineColor = Color.Black,
                OutlineThickness = 5,
            };

        }

        public virtual void Draw(RenderTarget target, RenderStates states)
        {
            if (IsInFocus)
                _focusShape.Draw(target, states);
        }

        public abstract FloatRect GetGlobalBounds();

        public virtual bool Intersect(Vector2i point, out Vector2f offset)
        {
            var bounds = GetGlobalBounds();
            offset = bounds.Position - new Vector2f(point.X, point.Y);

            return bounds.Contains(point.X, point.Y);

        }

        public virtual void OnFocus()
        {
            IsInFocus = true;
            //ZIndex = 0;
            //Console.WriteLine(GetTotalArea());
            UpdateFocusShape();
        }

        public virtual void OnLostFocus()
        {
            IsInFocus = false;
        }

        public virtual void OnMouseLeave() { }
        public virtual void OnMouseOver() { }

        public virtual float GetTotalArea()
        {
            var bounds = GetGlobalBounds();
            return bounds.Size.X * bounds.Size.Y;
        }

        protected void UpdateFocusShape()
        {
            if (!IsInFocus) return;
            Vector2f actualSize = (this is InteractiveComponentBase interactive)
                ? interactive.GetSize()
                : new Vector2f(100f, 100f);

            _focusShape.Position = Position;
            _focusShape.Size = actualSize;
            _focusShape.Origin = actualSize / 2f;
            _focusShape.Rotation = Shape.Rotation;

            float thickness = Math.Clamp(
                Math.Min(actualSize.X, actualSize.Y) * 0.05f, 2f, 10f
            );
            _focusShape.OutlineThickness = thickness;
        }

    }
}
