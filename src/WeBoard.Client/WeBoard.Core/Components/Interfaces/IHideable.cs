namespace WeBoard.Core.Components.Interfaces
{
    public interface IHidden
    {
        public bool IsHidden { get;}
        public void Show();
        public void Hide();
    }
}
