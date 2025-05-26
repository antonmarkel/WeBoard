using SFML.Graphics;
using SFML.System;
using WeBoard.Core.Components.Base;

namespace WeBoard.Core.Components.Menu.Visuals
{
    public class LabelComponent : MenuComponentBase
    {
        private readonly Text _text;
        private readonly Font _font;

        private readonly RectangleShape _shape = new();
        protected override Shape Shape => _shape;

        public Color BackgroundColor
        {
            get => _shape.FillColor;
            set => _shape.FillColor = value;
        }

        public string Content
        {
            get => _text.DisplayedString;
            set => _text.DisplayedString = value;
        }

        public override Vector2f Position
        {
            get => _text.Position;
            set
            {
                _text.Position = value;
                base.Position = value;
            }
        }

        public override FloatRect GetGlobalBounds() => _text.GetGlobalBounds();

        public LabelComponent(string content, Vector2f position, uint fontSize = 24)
        {
            _font = new Font("C:/Windows/Fonts/arial.ttf");
            _text = new Text(content, _font, fontSize)
            {
                FillColor = Color.Black,
                Position = position
            };

            _shape.Size = _text.GetGlobalBounds().Size * 1.05f;
            BackgroundColor = Color.Transparent;
            _shape.Position = position;
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            target.Draw(_shape);
            target.Draw(_text, states);
        }

        public override FloatRect GetScreenBounds()
        {
            return _text.GetGlobalBounds();
        }

        public override void OnClick(Vector2f offset)
        {
           
        }
    }
}
