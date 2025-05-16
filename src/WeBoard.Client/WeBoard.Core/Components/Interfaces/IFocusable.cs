using SFML.System;

namespace WeBoard.Core.Components.Interfaces;
public interface IFocusable
{
    public bool IsInFocus { get; set; }
    public void OnFocus();
    public void OnLostFocus();
    public void OnMouseOver();
    public void OnMouseLeave();
    public bool Intersect(Vector2i point, out Vector2f offset);
}

