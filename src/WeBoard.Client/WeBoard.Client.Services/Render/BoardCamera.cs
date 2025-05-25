using SFML.Graphics;
using SFML.System;
using SFML.Window;
using WeBoard.Client.Services.Managers;
using WeBoard.Core.Components.Interfaces;

namespace WeBoard.Client.Services.Render
{
    public class BoardCamera
    {
        private View _cameraView;
        private View _uiView;
        private float _zoomLevel = 1.0f;
        private const float ZoomStep = 0.1f;
        public View CameraView => new(_cameraView);
        public View UiView => _uiView;

        public BoardCamera(RenderWindow window)
        {
            _cameraView = new View(new Vector2f(0, 0),
                new Vector2f(window.Size.X, window.Size.Y));

            _uiView = new View(window.DefaultView);

            window.Resized += HandleResize;
            window.SetMouseCursorVisible(false);
        }

        private void HandleResize(object? sender, SizeEventArgs e)
        {
            var menuItems = ComponentManager.GetInstance().GetMenuComponents();
            foreach (var menuItem in menuItems)
            {
                menuItem.AdjustToResolution(e.Width,e.Height);
            }

            _cameraView.Size = new Vector2f(e.Width, e.Height);
            _cameraView.Zoom(_zoomLevel);

            _uiView.Size = new Vector2f(e.Width, e.Height);
            _uiView.Center = new Vector2f(e.Width / 2f, e.Height / 2f);
        }

        public void Zoom(float delta)
        {
            if (MouseManager.GetInstance().IsDragging)
                return;

            _zoomLevel *= delta > 0 ? 1 - ZoomStep : 1 + ZoomStep;
            _zoomLevel = Math.Clamp(_zoomLevel, 0.1f, 5.0f);

            _cameraView.Zoom(delta > 0 ? 1 - ZoomStep : 1 + ZoomStep);
        }

        public void MoveCamera(Vector2f offset)
        {
            _cameraView.Center += offset;
        }
    }
}
