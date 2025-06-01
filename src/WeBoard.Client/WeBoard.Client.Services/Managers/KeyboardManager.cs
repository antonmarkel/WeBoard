using SFML.Window;
using WeBoard.Core.Components.Menu.Inputs;
using WeBoard.Core.Components.Visuals;

namespace WeBoard.Client.Services.Managers
{
    public class KeyboardManager
    {
        private static readonly KeyboardManager Instance = new();

        private readonly RenderManager _global = RenderManager.GetInstance();
        private bool _isTextMode = false;
        public bool IsInTextMode() => _isTextMode;
        public void ExitTextMode() => _isTextMode = false;


        public static KeyboardManager GetInstance() => Instance;

        public KeyboardManager()
        {
            _global.RenderWindow.KeyPressed += OnKeyPressed;
            _global.RenderWindow.TextEntered += OnTextEntered;

        }

        private void OnKeyPressed(object? sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyPressed(Keyboard.Key.LControl) || Keyboard.IsKeyPressed(Keyboard.Key.RControl))
            {
                if (e.Code == Keyboard.Key.V)
                    PasteImageFromClipboard();
                else if (e.Code == Keyboard.Key.Z)
                    UpdateManager.GetInstance().RemoveLastUpdate();
            }
            if (e.Code == Keyboard.Key.T)
            {
                var focused = FocusManager.GetInstance().FocusedComponent;
                if (focused is not TextComponent text || !text.IsEditing)
                {
                    _isTextMode = true;
                }
            }
        }

        private void OnTextEntered(object? sender, TextEventArgs e)
        {
            var focused = FocusManager.GetInstance().FocusedComponent;
            if (focused is TextComponent textComponent && textComponent.IsEditing)
            {
                if (e.Unicode == "\b")
                {
                    textComponent.Backspace();
                }
                else if (e.Unicode == "\r" || e.Unicode == "\n")
                {
                    textComponent.AppendChar("\n");
                }
                else
                {
                    textComponent.AppendChar(e.Unicode);
                }
            }

            if (focused is TextInputComponent textInput && textInput.IsEditing)
            {
                if (e.Unicode == "\b")
                    textInput.Backspace();
                else if (e.Unicode != "\r" && e.Unicode != "\n")
                    textInput.AppendChar(e.Unicode);
            }
        }

        private void PasteImageFromClipboard()
        {
            Thread staThread = new Thread(() =>
             {
                 if (!System.Windows.Clipboard.ContainsImage())
                     return;

                 var bitmapSource = System.Windows.Clipboard.GetImage();
                 if (bitmapSource == null)
                     return;

                 var encoder = new System.Windows.Media.Imaging.BmpBitmapEncoder();
                 encoder.Frames.Add(System.Windows.Media.Imaging.BitmapFrame.Create(bitmapSource));

                 using var memoryStream = new System.IO.MemoryStream();
                 encoder.Save(memoryStream);
                 memoryStream.Position = 0;

                 var sfmlImage = new SFML.Graphics.Image(memoryStream);
                 var texture = new SFML.Graphics.Texture(sfmlImage);

                 var imageComponent = new ImageComponent(texture)
                 {
                     Position = new SFML.System.Vector2f(100, 100)
                 };

                 ComponentManager.GetInstance().AddComponent(imageComponent);
                 FocusManager.GetInstance().HandleClick(imageComponent.Position);
             });

            staThread.SetApartmentState(ApartmentState.STA);
            staThread.Start();
            staThread.Join();
        }
    }
}
