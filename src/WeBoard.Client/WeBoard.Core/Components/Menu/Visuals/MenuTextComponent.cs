using SFML.Graphics;
using SFML.System;
using WeBoard.Core.Components.Base;

namespace WeBoard.Core.Components.Menu.Visuals
{
    public class MenuTextComponent : ComponentBase
    {
        private readonly Text _text;
        private readonly Font _font;

        private readonly RectangleShape _shape = new();
        protected override Shape Shape => _shape;

        public string Content
        {
            get => _text.DisplayedString;
            set => _text.DisplayedString = value;
        }

        public override Vector2f Position
        {
            get => _text.Position;
            set => _text.Position = value;
        }

        public override FloatRect GetGlobalBounds() => _text.GetGlobalBounds();

        public MenuTextComponent(string content, Vector2f position, uint fontSize = 24)
        {
            _font = new Font("C:/Windows/Fonts/arial.ttf");
            _text = new Text(content, _font, fontSize)
            {
                FillColor = Color.Black,
                Position = position
            };
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            target.Draw(_text, states);
        }
    }
}
