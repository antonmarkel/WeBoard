using SFML.Graphics;
using SFML.System;
using System.Drawing;
using WeBoard.Core.Components.Interfaces;

namespace WeBoard.Core.Components.Content
{
    public class ImageContentView : IViewContent
    {
        private Sprite _sprite;

        public ImageContentView(Texture texture)
        {
            _sprite = new Sprite(texture);
            // Set origin to center for proper rotation
            _sprite.Origin = new Vector2f(
                _sprite.TextureRect.Width / 2f,
                _sprite.TextureRect.Height / 2f
            );
            Size = new Vector2f(_sprite.TextureRect.Width, _sprite.TextureRect.Height);
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            // Apply transformations
            _sprite.Position = Position;
            _sprite.Rotation = Rotation;
            _sprite.Scale = new Vector2f(
                Size.X / _sprite.TextureRect.Width,
                Size.Y / _sprite.TextureRect.Height
            );

            target.Draw(_sprite, states);
        }

        public override void SetSize(Vector2f size)
        {
            base.SetSize(size);
            // Additional logic if needed (e.g., maintain aspect ratio)
        }
    }
}
