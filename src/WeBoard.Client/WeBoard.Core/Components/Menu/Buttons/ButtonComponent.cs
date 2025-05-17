using SFML.Graphics;
using SFML.System;
using WeBoard.Core.Components.Base;
using WeBoard.Core.Components.Interfaces;

namespace WeBoard.Core.Components.Menu.Buttons
{
    public class ButtonComponent : MenuComponentBase, IContainerView
    {
        private RectangleShape _buttonShape;
        public IContentView? ContentView { get; set; }
        public uint Padding { get; set; }

        public override Vector2f Position
        {
            get => _buttonShape.Position;
            set => _buttonShape.Position = value;
        }
        public Vector2f Size
        {
            get => _buttonShape.Size;
            set => _buttonShape.Size = value;
        }
        public Color OutlineColor
        {
            get => _buttonShape.OutlineColor;
            set => _buttonShape.OutlineColor = value;
        }
        public float OutlineThickness
        {
            get => _buttonShape.OutlineThickness;
            set => _buttonShape.OutlineThickness = value;
        }
        public Color BackgroundColor
        {
            get => _buttonShape.FillColor;
            set => _buttonShape.FillColor = value;
        }

        protected override Shape Shape => _buttonShape;

        public ButtonComponent(Vector2f position, Vector2f size)
        {
            _buttonShape = new RectangleShape();

            Position = position;
            Size = size;
            OutlineThickness = 2;
            OutlineColor = Color.Black;
            BackgroundColor = new Color(0, 0, 0, 120);
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {


            _buttonShape.Draw(target, states);
            ContentView?.Draw(target, states);
        }

        private void UpdateContentView()
        {
            if (ContentView is not null)
            {
                ContentView.Size = Size - new Vector2f(Padding, Padding);
                Position += new Vector2f(Padding, Padding);
            }
        }

        public override FloatRect GetScreenBounds()
        {
            return _buttonShape.GetLocalBounds();
        }

        public override void OnLostFocus()
        {
            if (IsInFocus)
            {
                OutlineThickness -= 2;
            }
            base.OnLostFocus();
        }

        public override void OnFocus()
        {
            if (!IsInFocus)
            {
                OutlineThickness += 2;
            }
            base.OnLostFocus();
        }

        public override void OnClick()
        {
            Console.WriteLine("Clicked");
        }
    }
}
