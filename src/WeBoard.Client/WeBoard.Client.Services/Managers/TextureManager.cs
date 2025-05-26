using SFML.Graphics;

namespace WeBoard.Client.Services.Managers
{
    public class TextureManager
    {
        private static TextureManager? Instance;
        public TextureManager() { }
        public static TextureManager GetInstance()
        {
            return Instance ?? (Instance = new());
        }

        public Texture MenuBrush = new("Resources/Menu/brush.png");
        public Texture MenuCursor = new("Resources/Menu/cursor.png");
        public Texture MenuEraser = new("Resources/Menu/eraser.png");
        public Texture MenuFont = new("Resources/Menu/font.png");
        public Texture MenuPencil = new("Resources/Menu/pencil.png");
    }
}
