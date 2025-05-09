using SFML.System;
using WeBoard.Core.Components.Interfaces;

namespace WeBoard.Core.Components.Base
{
    public abstract class MenuComponentBase : ComponentBase, IMenuComponent
    {
        public virtual bool IsVisible { get; set; }
        public virtual void Hide()
        {
            IsVisible = false;
        }

        public void Show()
        {
            IsVisible = true;
        }

        public override bool Intersect(Vector2i point, out Vector2f offset)
        {
            var bounds = GetLocalBounds();
            offset = bounds.Position - new Vector2f(point.X, point.Y);

            return bounds.Contains(point.X, point.Y);
        }
    }
}
