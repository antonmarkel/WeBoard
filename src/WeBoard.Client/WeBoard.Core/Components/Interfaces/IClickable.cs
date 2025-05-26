using SFML.System;

namespace WeBoard.Core.Components.Interfaces
{
    public interface IClickable
    {
        public void OnClick(Vector2f offset);
    }
}
