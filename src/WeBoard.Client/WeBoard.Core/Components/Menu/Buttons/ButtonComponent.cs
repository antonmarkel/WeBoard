using SFML.Graphics;
using SFML.System;
using SFML.Window;
using WeBoard.Core.Components.Base;
using WeBoard.Core.Components.Interfaces;

namespace WeBoard.Core.Components.Menu.Buttons
{
    public class ButtonComponent : MenuComponentBase, IContainerView
    {
        private RectangleShape _buttonShape;
        public override Vector2f Position { get; set; }
        public Vector2f Size { get; set; }
        public IContentView? ContentView { get; set; }
        public uint Padding { get; set; }

        public Color OutlineColor {
            get => _buttonShape.OutlineColor;
            set => _buttonShape.OutlineColor = value;
        }
        public float OutlineThickness
        {
            get => _buttonShape.OutlineThickness;
            set => _buttonShape.OutlineThickness = value;
        }
        public Color BackgroundColor { 
            get => _buttonShape.FillColor;
            set => _buttonShape.FillColor = value;
        }

        protected override Shape Shape => _buttonShape;

        public ButtonComponent(Vector2f position,Vector2f size)
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
                ContentView.Size = Size - new Vector2f(Padding,Padding);
                Position = Position + new Vector2f(Padding,Padding);
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
