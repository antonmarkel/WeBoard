using SFML.System;
using WeBoard.Core.Components.Base;
using WeBoard.Core.Components.Menu.Containers.Base;

namespace WeBoard.Core.Components.Menu.Containers
{
    public class HorizontalStackContainer : StackContainerBase
    {
        public HorizontalStackContainer(IEnumerable<MenuComponentBase> components) : base(components)
        {
        }

        public override void OnClick(Vector2f offset)
        {
            
        }

        protected override void UpdateContainer()
        {
            float width = _padding.Y;
            float height = 0;
            foreach (var child in _children)
            {
                var bounds = child.GetScreenBounds();
                height = Math.Max(height, bounds.Height);
                child.Position = Position + new Vector2f(width, _padding.Y);
                width += bounds.Width + SpaceBetween;
            }

            height += 2 * _padding.Y;
            width += _padding.X - SpaceBetween;

            _focusShape.Size = _bodyShape.Size = new Vector2f(width, height);
        }
    }
}
