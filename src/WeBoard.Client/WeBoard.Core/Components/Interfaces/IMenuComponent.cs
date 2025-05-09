using WeBoard.Core.Components.Base;

namespace WeBoard.Core.Components.Interfaces
{
    public interface IMenuComponent : IComponent, IFocusable, IClickable
    {
        public bool IsVisible { get; set; }
        public void Hide();
        public void Show();
    }
}
