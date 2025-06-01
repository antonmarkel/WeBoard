using SFML.Graphics;
using SFML.System;

namespace WeBoard.Core.Drawables.Cursors
{
    public class RemoteCursor : Drawable
    {
        private readonly CircleShape _cursorShape;
        private readonly Text _nameText;
        private float _sizeCf = 1f;
        public float SecondsSinceUpdate { get; private set; }

        public RemoteCursor(string userName, Color color, Font font)
        {
            _cursorShape = new CircleShape(15f)
            {
                FillColor = color,
                OutlineColor = Color.Black,
                OutlineThickness = 2f
            };

            _nameText = new Text(userName, font, 25)
            {
                FillColor = color,
                Style = Text.Styles.Bold
            };
        }

        public void Update(Vector2f position,float sizeCf = 1f)
        {
            SecondsSinceUpdate = 0;
            _cursorShape.Position = position;
            _nameText.Position = position + new Vector2f(30f, -20f);
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            target.Draw(_cursorShape);
            target.Draw(_nameText);
        }
    }
}
