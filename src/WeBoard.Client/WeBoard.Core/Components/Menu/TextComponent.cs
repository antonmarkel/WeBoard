using SFML.Graphics;
using SFML.System;
using WeBoard.Core.Components.Base;

namespace WeBoard.Core.Components.Menu
{
    public class TextComponent : MenuComponentBase
    {
        private readonly Text _text;
        private bool _isMouseOver = false;

        public override Vector2f Position { 
            get => _text.Position; 
            set => _text.Position = value; 
        }
        public uint Size
        {
            get => _text.CharacterSize;
            set => _text.CharacterSize = value;
        }
        public TextComponent(Vector2f position, string text) : base()
        {
            var font = new Font(@"C:\Windows\Fonts\RAVIE.ttf");
            _text = new Text(text, font, 25);
            _text.Position = position;
            _text.FillColor = Color.White;
            _text.Style = Text.Styles.Bold;
            Size = 25;

            _focusShape.FillColor = new Color(0, 0, 0, 120);
        }

        public override FloatRect GetGlobalBounds()
        {
            return _text.GetGlobalBounds();
        }

        public override FloatRect GetLocalBounds()
        {
            return _text.GetGlobalBounds();
        }

        public override void OnMouseOver()
        {
            _isMouseOver = true;
            var bounds = GetGlobalBounds();
            _focusShape.Position = bounds.Position;
            _focusShape.Size = bounds.Size;
        }
        public override void OnMouseLeave()
        {
            _isMouseOver = false;
        }


        public override void Draw(RenderTarget target, RenderStates states)
        {
            _text.Draw(target, states);
            if (_isMouseOver)
                _focusShape.Draw(target, states);
        }
    }
}
