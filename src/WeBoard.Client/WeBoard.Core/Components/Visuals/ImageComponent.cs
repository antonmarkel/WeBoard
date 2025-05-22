using SFML.Graphics;
using SFML.System;
using WeBoard.Core.Components.Base;

namespace WeBoard.Core.Components.Visuals
{
    public class ImageComponent : InteractiveComponentBase
    {
        private Sprite _sprite;
        private RectangleShape _focusRectangle;
        protected override Shape Shape => _focusRectangle;

        public ImageComponent(Texture texture)
        {
            _sprite = new Sprite(texture);
            _focusRectangle = new RectangleShape();
            var originalSize = new Vector2f(texture.Size.X, texture.Size.Y);
            
            Position = new Vector2f(100, 100);
            SetSize(originalSize);

            _focusRectangle.FillColor = Color.Transparent;

            UpdateSpriteScale();
        }

        public override Vector2f Position
        {
            get => _sprite.Position;
            set
            {
                _sprite.Position = value;
                _focusRectangle.Position = value;
            }
        }

        private float _rotation;

        public override float Rotation
        {
            get => _rotation;
            set
            {
                _rotation = value;
                _focusRectangle.Rotation = value;
                _sprite.Rotation = value;
            }
        }

        public override Vector2f GetSize()
        {
            return _focusRectangle.Size;
        }

        public override void SetSize(Vector2f size)
        {
            _focusRectangle.Size = new Vector2f(
                Math.Max(size.X, MinWidth),
                Math.Max(size.Y, MinHeight)
            );
            _focusRectangle.Origin = _focusRectangle.Size / 2f;

            UpdateSpriteScale();

            UpdateHandles();       
            UpdateFocusShape();   
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            target.Draw(_sprite, states);
            target.Draw(_focusRectangle, states);
            base.Draw(target, states);
        }

        private void UpdateSpriteScale()
        {
            var texSize = new Vector2f(_sprite.Texture.Size.X, _sprite.Texture.Size.Y);
            _sprite.Scale = new Vector2f(
                _focusRectangle.Size.X / texSize.X,
                _focusRectangle.Size.Y / texSize.Y
            );
            _sprite.Origin = texSize / 2f;
            _sprite.Position = _focusRectangle.Position;
            _sprite.Rotation = Rotation;
        }
    }
}
