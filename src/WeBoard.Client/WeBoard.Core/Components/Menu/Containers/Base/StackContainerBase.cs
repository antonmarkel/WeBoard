using System.Collections.Immutable;
using SFML.Graphics;
using SFML.System;
using WeBoard.Core.Components.Base;
using WeBoard.Core.Drawables.Shapes;

namespace WeBoard.Core.Components.Menu.Containers.Base
{
    public abstract class StackContainerBase : MenuComponentBase
    {
        protected RoundedRectangle _bodyShape;
        protected RectangleShape _focusShape;
        protected ImmutableList<MenuComponentBase> _children;
        protected Vector2f _padding;
        protected float _spaceBetween;

        public StackContainerBase(IEnumerable<MenuComponentBase> components)
        {
            _children = components.ToImmutableList();
            _children.ForEach(child =>
            {
                child.Parent = this;
                child.ZIndex = ZIndex + 1;
            });

            _focusShape = new RectangleShape();
            _bodyShape = new RoundedRectangle();

            _padding = new Vector2f(0, 0);
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

        protected abstract void UpdateContainer();

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

        public override void AdjustToResolution(uint width, uint height)
        {
            UpdateContainer();
        }


    }
}
