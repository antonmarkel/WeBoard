using SFML.System;
using WeBoard.Core.Enums;

namespace WeBoard.Core.Components.Interfaces
{
    public interface IResizable
    {
        public void OnStartResizing() { }
        public void OnFinishResizing() { }
        void Resize(Vector2f delta, ResizeDirectionEnum direction);
        Vector2f GetSize();
        void SetSize(Vector2f size);
        float MinWidth { get; }
        float MinHeight { get; }
    }
}
