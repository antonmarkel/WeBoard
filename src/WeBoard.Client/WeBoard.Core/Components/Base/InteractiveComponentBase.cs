using SFML.System;
using WeBoard.Core.Components.Interfaces;

namespace WeBoard.Core.Components.Base
{
    public abstract class InteractiveComponentBase : ComponentBase, IDraggable, IFocusable
    {
        public abstract void Drag(Vector2f offset);
        public virtual void OnFocus()
        {
            IsInFocus = true;
            Console.WriteLine(GetTotalArea());
            _focusShape.Size = GetGlobalBounds().Size;
            _focusShape.Position = GetGlobalBounds().Position;
            _focusShape.OutlineThickness = Math.Max(GetTotalArea() / (1 * 50_000), 1);
        }

        public virtual void OnLostFocus()
        {
            IsInFocus = false;
        }

        public virtual void OnMouseLeave() { }
        public virtual void OnMouseOver() { }
    }
}
