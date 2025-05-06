using SFML.Graphics;
using SFML.System;
using SFML.Window;
using WeBoard.Client.Core.Engine;
using WeBoard.Client.Services.Managers;

namespace WeBoard.Client.Services.Render
{
    public class BoardCamera
    {
        private View _cameraView;
        private float _zoomLevel = 1.0f;
        private const float ZoomStep = 0.1f;

        public View CameraView => new(_cameraView);

        public BoardCamera(RenderWindow window)
        {
            _cameraView = new View(new Vector2f(0, 0),
                new Vector2f(window.Size.X, window.Size.Y));

            window.Resized += HandleResize;
        }

        private void HandleResize(object? sender, SizeEventArgs e)
        {
            _cameraView.Size = new Vector2f(e.Width, e.Height);
            _cameraView.Zoom(_zoomLevel);
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
            //var mouseManager = MouseManager.GetInstance();

            //if (mouseManager || FocusManager.GetInstance().FocusedComponent != null)
            //    return;

            //var currentScreen = new Vector2i(e.X, e.Y);
            //var currentWorld = _global.RenderWindow.MapPixelToCoords(currentScreen, _initialView);
            //var offset = _dragStartWorld - currentWorld;

            _cameraView.Center += offset;
        }

        //private void HandleMouseRelease(object? sender, MouseButtonEventArgs e)
        //{
        //    if (e.Button == Mouse.Button.Left)
        //        _isDragging = false;
        //}

        //private void HandleMousePress(object? sender, MouseButtonEventArgs e)
        //{
        //    if (e.Button == Mouse.Button.Left)
        //    {
        //        var coords = BoardGlobal.GetInstance().RenderWindow!.MapPixelToCoords(new Vector2i(e.X, e.Y));
        //FocusManager.GetInstance().HandleClick(new Vector2f((int) coords.X, (int) coords.Y));
        //        if (FocusManager.GetInstance().FocusedComponent != null)
        //            return;

        //        _dragStartScreen = new Vector2i(e.X, e.Y);
        //_initialView = new View(_cameraView);
        //_initialViewCenter = _cameraView.Center;
        //        _dragStartWorld = _window.MapPixelToCoords(_dragStartScreen, _initialView);

        //        _isDragging = true;
        //    }

    //}
}
}
