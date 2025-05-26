using SFML.System;
using WeBoard.Core.Enums.Menu;

namespace WeBoard.Client.Services.Managers
{
    public class MenuManager
    {
        private static MenuManager? Instance;

        private readonly CursorManager _cursorManager = CursorManager.GetInstance();
        private readonly TextureManager _textureManager = TextureManager.GetInstance();
        public MenuManager() { }
        public static MenuManager GetInstance()
        {
            return Instance ?? (Instance = new());
        }

        private InstrumentOptionsEnum _currentInstrument = InstrumentOptionsEnum.Cursor;
        public InstrumentOptionsEnum CurrentInstrument
        {
            get => _currentInstrument;
            set
            {
                _currentInstrument = value;
                UpdateCursorTexture();
            }
        }

        private void UpdateCursorTexture()
        {
            switch (CurrentInstrument)
            {
                case InstrumentOptionsEnum.Cursor:
                    _cursorManager.CursorTexture = _textureManager.MenuCursor;
                    _cursorManager.Origin = new Vector2f(120, 40);
                    break;
                case InstrumentOptionsEnum.Brush:
                    _cursorManager.CursorTexture = _textureManager.MenuBrush;
                    _cursorManager.Origin = new Vector2f(50, 442);
                    break;
                case InstrumentOptionsEnum.Pencil:
                    _cursorManager.CursorTexture = _textureManager.MenuPencil;
                    _cursorManager.Origin = new Vector2f(18, 231);
                    break;
                case InstrumentOptionsEnum.Eraser:
                    _cursorManager.CursorTexture = _textureManager.MenuEraser;
                    _cursorManager.Origin = new Vector2f(115, 471);
                    break;
                case InstrumentOptionsEnum.Text:
                    _cursorManager.CursorTexture = _textureManager.MenuFont;
                    _cursorManager.Origin = new Vector2f(126, 105);
                    break;
                default:
                    _cursorManager.CursorTexture = _textureManager.MenuCursor;
                    _cursorManager.Origin = new Vector2f(120, 40);
                    break;
            }
        }
    }
}
