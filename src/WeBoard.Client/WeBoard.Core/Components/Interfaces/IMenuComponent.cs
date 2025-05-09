namespace WeBoard.Core.Components.Interfaces
{
    public interface IMenuComponent : IComponent, IClickable, IMouseDetective
    {
        public bool IsVisible { get; set; }
        public void Hide();
        public void Show();
    }
}
