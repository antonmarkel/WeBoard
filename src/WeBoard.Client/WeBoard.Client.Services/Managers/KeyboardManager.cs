﻿using SFML.Window;
using WeBoard.Core.Components.Content;

namespace WeBoard.Client.Services.Managers
{
    public class KeyboardManager
    {
        private static readonly KeyboardManager Instance = new();

        private readonly RenderManager _global = RenderManager.GetInstance();

        public static KeyboardManager GetInstance() => Instance;

        public KeyboardManager()
        {
            _global.RenderWindow.KeyPressed += OnKeyPressed;
        }

        private void OnKeyPressed(object? sender, KeyEventArgs e)
        {
            if (e.Code == Keyboard.Key.V &&
                (Keyboard.IsKeyPressed(Keyboard.Key.LControl) || Keyboard.IsKeyPressed(Keyboard.Key.RControl)))
            {
                PasteImageFromClipboard();
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
