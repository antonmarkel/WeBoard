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
    }
}
