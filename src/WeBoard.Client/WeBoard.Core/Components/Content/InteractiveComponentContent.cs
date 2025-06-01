using SFML.Graphics;
using SFML.System;
using WeBoard.Core.Components.Base;
using WeBoard.Core.Components.Content.Base;

namespace WeBoard.Core.Components.Content
{
    public class InteractiveComponentContent : ContentViewBase
    {
        private InteractiveComponentBase _component;
        public InteractiveComponentContent(InteractiveComponentBase component)
        {
            _component = component;
        }

        public override Vector2f Position
        {
            get => _component.Position;
            set => _component.Position = value + _component.GetSize() / 2f;
        }
        public override float Rotation
        {
            get => _component.Rotation;
            set => _component.Rotation = value;
        }
        public override Vector2f Size
        {
            get => _component.GetSize();
            set => _component.SetSize(value);
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            _component.Draw(target, states);
        }
    }
}
