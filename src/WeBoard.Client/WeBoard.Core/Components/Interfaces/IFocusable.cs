namespace WeBoard.Core.Components.Interfaces;
public interface IFocusable
{
    public bool IsInFocus { get; set; }
    public void OnFocus();
    public void OnLostFocus();
}

