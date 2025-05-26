namespace WeBoard.Core.Components.Interfaces
{
    public interface ITextCursorControllable
    {
        void SetCursorVisible(bool visible);
        bool IsEditing { get; }
    }
}
