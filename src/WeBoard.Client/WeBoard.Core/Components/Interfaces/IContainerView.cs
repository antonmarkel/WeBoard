namespace WeBoard.Core.Components.Interfaces
{
    public interface IContainerView
    {
        public IContentView? ContentView { get; set; }
        public uint Padding { get; set; }
    }
}
