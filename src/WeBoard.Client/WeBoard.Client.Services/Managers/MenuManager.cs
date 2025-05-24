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

        private InstrumentOptionsEnum _currentInstrument;
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
                    break;
                case InstrumentOptionsEnum.Brush:
                    _cursorManager.CursorTexture = _textureManager.MenuBrush;
                    break;
                case InstrumentOptionsEnum.Pencil:
                    _cursorManager.CursorTexture = _textureManager.MenuPencil;
                    break;
                case InstrumentOptionsEnum.Eraser:
                    _cursorManager.CursorTexture = _textureManager.MenuEraser;
                    break;
                case InstrumentOptionsEnum.Text:
                    _cursorManager.CursorTexture = _textureManager.MenuFont;
                    break;
                default:
                    _cursorManager.CursorTexture = _textureManager.MenuCursor;
                    break;
            }
        }
    }
}
