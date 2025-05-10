using SFML.Graphics;
using SFML.System;
using WeBoard.Core.Components.Base;
using WeBoard.Core.Components.Interfaces;

namespace WeBoard.Core.Components.Menu
{
    public class ScrollViewComponent : MenuComponentBase, IComplexMenuComponent, IScrollable
    {
        private readonly RenderTexture _contentTexture;
        private readonly Sprite _contentSprite;
        private readonly ScrollBarComponent _scrollBar;
        private readonly View _contentView;
        private float _contentHeight;
        private Vector2f _size;
        private readonly List<MenuComponentBase> _children = new();

        public ScrollViewComponent(Vector2f position, Vector2f size)
        {
            _size = size;
            Position = position;

            _contentTexture = new RenderTexture((uint)size.X, (uint)size.Y);
            _contentSprite = new Sprite(_contentTexture.Texture)
            {
                Position = position
            };
            _contentView = new View(new FloatRect(0, 0, size.X, size.Y));
            _contentTexture.SetView(_contentView);

            _scrollBar = new ScrollBarComponent(new Vector2f(Position.X + size.X, Position.Y),
                size.Y);

        }

        public void AddChild(MenuComponentBase component)
        {
            _children.Add(component);
            UpdateContentSize();
        }

        private void UpdateContentSize()
        {
            _contentHeight = _children.Max(c => c.GetGlobalBounds().Top + c.GetGlobalBounds().Height);

            float visibleRatio = _size.Y / _contentHeight;
            _scrollBar.ThumbSize = visibleRatio;
        }

        public void Scroll(float delta)
        {
            _scrollBar.Scroll(delta);
            var centerY = (_contentHeight - _size.Y) * _scrollBar.Value + (_size.Y / 2);

            _contentView.Center = new Vector2f(_contentView.Center.X, centerY);
            _contentTexture.SetView(_contentView);
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            _contentTexture.Clear(new Color(0,0,0,140));
            foreach (var child in _children)
            {
                child.Draw(_contentTexture, states);
            }
            _contentTexture.Display();

            target.Draw(_contentSprite, states);
            _scrollBar.Draw(target, states);

            base.Draw(target, states);
        }

        public override FloatRect GetGlobalBounds()
        {
            return new FloatRect(Position, _size);
        }

        public override FloatRect GetLocalBounds()
        {
            return new FloatRect(Position, _size);
        }

 
        public override bool Intersect(Vector2i point, out Vector2f offset)
        {
            bool intersects = GetGlobalBounds().Contains(point.X, point.Y);
            offset = intersects ? new Vector2f(point.X, point.Y) - Position : new Vector2f(0,0);
            return intersects;
        }

        public MenuComponentBase? ChildUnderMouse(Vector2f offset, out Vector2f inOffset)
        {
            var localPosition = offset + _contentView.Center - new Vector2f(_size.X / 2, _size.Y / 2);
            Vector2f childOffset = new Vector2f(0, 0);
            var child = _children.FirstOrDefault(comp => comp.Intersect(new Vector2i((int)localPosition.X, (int)localPosition.Y), out childOffset));

            inOffset = childOffset;

            return child;
        }
    }

}
