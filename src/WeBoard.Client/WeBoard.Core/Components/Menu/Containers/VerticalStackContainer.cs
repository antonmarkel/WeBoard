using SFML.System;
using WeBoard.Core.Components.Base;
using WeBoard.Core.Components.Menu.Containers.Base;

namespace WeBoard.Core.Components.Menu.Containers
{
    public class VerticalStackContainer : StackContainerBase
    {
        public VerticalStackContainer(IEnumerable<MenuComponentBase> components) : base(components)
        {

        }

        public override void OnClick(Vector2f offset)
        {

        }

        protected override void UpdateContainer()
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

            FocusShape.Size = _bodyShape.Size = new Vector2f(width, height);
        }
    }
}
