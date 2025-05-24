using SFML.Graphics;
using SFML.System;

namespace WeBoard.Client.Services.Managers
{
    public class CursorManager : Drawable
    {
        private static CursorManager? Instance;
        private const int CursorSize = 40;
        public CursorManager()
        {
            _texture = TextureManager.GetInstance().MenuCursor;
            _sprite = new Sprite(_texture);
        }
        public static CursorManager GetInstance()
        {
            return Instance ?? (Instance = new());
        }

        private Sprite _sprite;
        private Texture _texture;
        public Texture CursorTexture
        {
            get => _texture;
            set
            {
                _texture = value;
                _sprite.Texture = _texture;
                _sprite.Scale = new Vector2f(
                    (float)CursorSize / _texture.Size.X,
                    (float)CursorSize / _texture.Size.Y
                );
            }
        }

        public void SetPosition(Vector2i position)
        {
            _sprite.Position = new Vector2f(position.X, position.Y);
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            _sprite.Draw(target, states);
        }
    }
}
