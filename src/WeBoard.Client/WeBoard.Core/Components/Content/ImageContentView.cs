using SFML.Graphics;
using SFML.System;
using System.Drawing;
using WeBoard.Core.Components.Content.Base;
using WeBoard.Core.Components.Interfaces;

namespace WeBoard.Core.Components.Content
{
    public class ImageContentView : ContentViewBase
    {
        private Sprite _sprite;

        public ImageContentView(Texture texture)
        {
            _sprite = new Sprite(texture);
            Size = new Vector2f(_sprite.TextureRect.Width, _sprite.TextureRect.Height);
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            _sprite.Position = Position;
            _sprite.Rotation = Rotation;
            _sprite.Scale = new Vector2f(
                Size.X / _sprite.TextureRect.Width,
                Size.Y / _sprite.TextureRect.Height
            );

            target.Draw(_sprite, states);
        }
    }
}
