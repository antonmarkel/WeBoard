using SFML.Graphics;
using SFML.System;
using SFML.Window;
using WeBoard.Client.Core.Engine;

namespace WeBoard.Client.Board
{
    public class BoardCamera
    {
        private View _cameraView;
        private RenderWindow _window;
        private bool _isDragging = false;

        private Vector2f _dragStartWorld;
        private Vector2i _dragStartScreen;
        private View? _initialView = null;
        private Vector2f _initialViewCenter;

        private float _zoomLevel = 1.0f;
        private const float ZoomStep = 0.1f;

        public View CameraView => new(_cameraView);

        public BoardCamera(RenderWindow window)
        {
            _window = window;
            _cameraView = new View(new Vector2f(0, 0), new Vector2f(_window.Size.X, _window.Size.Y));
            _window.MouseButtonPressed += HandleMousePress;
            _window.MouseButtonReleased += HandleMouseRelease;
            _window.MouseMoved += HandleMouseMove;
            _window.MouseWheelScrolled += HandleMouseWheel;
        }

        private void HandleMouseWheel(object? sender, MouseWheelScrollEventArgs e)
        {
            _zoomLevel *= e.Delta > 0 ? 1 - ZoomStep : 1 + ZoomStep;
            _zoomLevel = Math.Clamp(_zoomLevel, 0.1f, 5.0f);

            _cameraView.Zoom(e.Delta > 0 ? 1 - ZoomStep : 1 + ZoomStep);

            if (_isDragging)
            {
                _initialView = new View(_cameraView);
                _initialViewCenter = _cameraView.Center;
            }

        }

        private void HandleMouseMove(object? sender, MouseMoveEventArgs e)
        {
            if (!_isDragging)
                return;

            var currentScreen = new Vector2i(e.X, e.Y);
            var currentWorld = _window.MapPixelToCoords(currentScreen, _initialView);

            var offset = _dragStartWorld - currentWorld;
            _cameraView.Center = _initialViewCenter + offset;
        }

        private void HandleMouseRelease(object? sender, MouseButtonEventArgs e)
        {
            if (e.Button == Mouse.Button.Left)
                _isDragging = false;
        }

        private void HandleMousePress(object? sender, MouseButtonEventArgs e)
        {
            if (e.Button == Mouse.Button.Left)
            {
                _dragStartScreen = new Vector2i(e.X, e.Y);

                _initialView = new View(_cameraView);
                _initialViewCenter = _cameraView.Center;

                _dragStartWorld = _window.MapPixelToCoords(_dragStartScreen, _initialView);

                _isDragging = true;
            }

        }
    }
}
