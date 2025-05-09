using SFML.Graphics;
using SFML.System;
using SFML.Window;
using WeBoard.Client.Services.Managers;

namespace WeBoard.Client.Services.Render
{
    public class BoardCamera
    {
        private View _contentView;
        private View _uiView;
        public float ZoomLevel { get => _zoomLevel; }
        private float _zoomLevel = 1.0f;
        private const float ZoomStep = 0.1f;

        public View CameraView => _contentView;
        public View UiView => _uiView;

        public BoardCamera(RenderWindow window)
        {
            _contentView = new View(new Vector2f(0, 0),
                new Vector2f(window.Size.X, window.Size.Y));

            _uiView = new View(window.DefaultView);

            window.Resized += HandleResize;
        }

        private void HandleResize(object? sender, SizeEventArgs e)
        {
            _contentView.Size = new Vector2f(e.Width, e.Height);
            _contentView.Zoom(_zoomLevel);

            _uiView.Size = new Vector2f(e.Width, e.Height);
            _uiView.Center = new Vector2f(e.Width / 2f, e.Height / 2f);
        }

        public void Zoom(float delta)
        {
            if (MouseManager.GetInstance().IsDragging)
                return;

            _zoomLevel *= delta > 0 ? 1 - ZoomStep : 1 + ZoomStep;
            _zoomLevel = Math.Clamp(_zoomLevel, 0.1f, 5.0f);

            _contentView.Zoom(delta > 0 ? 1 - ZoomStep : 1 + ZoomStep);
        }

        public void MoveCamera(Vector2f offset)
        {

            _contentView.Center += offset;
        }
    }
}
