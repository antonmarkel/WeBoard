using SFML.Graphics;
using SFML.System;
using WeBoard.Core.Components.Interfaces;

namespace WeBoard.Core.Components.Base
{
    public abstract class ComponentBase : IComponent, IFocusable, IClickable
    {
        protected RectangleShape _focusShape;
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
        public virtual Vector2f Position { get => _focusShape.Position; set => _focusShape.Position = value; }

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
        public abstract FloatRect GetLocalBounds();
        public virtual bool Intersect(Vector2i point, out Vector2f offset)
        {
            var bounds = GetGlobalBounds();
            offset = bounds.Position - new Vector2f(point.X, point.Y);

            return bounds.Contains(point.X, point.Y);

        }

        public virtual void OnFocus()
        {
            IsInFocus = true;
            Console.WriteLine(GetTotalArea());
            _focusShape.Size = GetGlobalBounds().Size;
            _focusShape.Position = GetGlobalBounds().Position;
            _focusShape.OutlineThickness = Math.Max(GetTotalArea() / (1 * 50_000) , 1);
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
    }
}
