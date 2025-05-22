using System.Collections.Immutable;
using SFML.Graphics;
using SFML.System;
using WeBoard.Core.Components.Base;
using WeBoard.Core.Drawables.Shapes;

namespace WeBoard.Core.Components.Menu.Containers
{
    public class VerticalStackContainer : MenuComponentBase
    {
        private RoundedRectangle _bodyShape;
        private RectangleShape _focusShape;
        private ImmutableList<MenuComponentBase> _children;
        private Vector2f _padding;
        private float _spaceBetween;

        public VerticalStackContainer(IEnumerable<MenuComponentBase> components)
        {
            _children = components.ToImmutableList();
            _children.ForEach(child =>
            {
                child.Parent = this;
                child.ZIndex = this.ZIndex + 1;
            });

            _focusShape = new RectangleShape();
            _bodyShape = new RoundedRectangle();
                
            _padding = new Vector2f(0,0);
            _spaceBetween = 0f;

            UpdateContainer();
        }

        public Color OutlineColor
        {
            get => _bodyShape.OutlineColor;
            set => _bodyShape.OutlineColor = value;
        }
        public float OutlineThickness
        {
            get => _bodyShape.OutlineThickness;
            set => _bodyShape.OutlineThickness = value;
        }
        public Color BackgroundColor
        {
            get => _bodyShape.FillColor;
            set => _bodyShape.FillColor = value;
        }
      
        public float CornerRadius
        {
            get => _bodyShape.CornerRadius;
            set => _bodyShape.CornerRadius = value;
        }

        public uint CornerPointCount
        {
            get => _bodyShape.CornerPointCount;
            set => _bodyShape.CornerPointCount = value;
        }

        public Vector2f Padding
        {
            get => _padding;
            set
            {
                _padding = value;
                UpdateContainer();
            }
        }

        public float SpaceBetween
        {
            get => _spaceBetween;
            set
            {
                _spaceBetween = value;
                UpdateContainer();
            }
        }
        public override Vector2f Position
        {
            get => _bodyShape.Position;
            set
            {
                _bodyShape.Position = _focusShape.Position = value;
                UpdateContainer();
            }
        }
 

        protected override Shape Shape => _focusShape;
        public override FloatRect GetScreenBounds()
        {
            return _focusShape.GetGlobalBounds();
        }

        private void UpdateContainer()
        {
            float width = 0;
            float height = _padding.X;
            foreach (var child in _children)
            {
                var bounds = child.GetScreenBounds();
                width = Math.Max(width, bounds.Width);
                child.Position = Position + new Vector2f(_padding.X, height);
                height += bounds.Height + SpaceBetween;
            }

            width += 2 * _padding.X;
            height += _padding.Y - SpaceBetween;

            _focusShape.Size = _bodyShape.Size = new Vector2f(width, height);
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            if (IsHidden)
                return;
            
            _bodyShape.Draw(target, states);
            foreach (var child in _children)
            {
                child.Draw(target, states);
            }
        }


        public override void OnClick(Vector2f offset)
        {
        }
    }
}
