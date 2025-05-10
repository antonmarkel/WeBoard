using SFML.Graphics;
using SFML.System;
using WeBoard.Core.Components.Handlers;
using WeBoard.Core.Components.Interfaces;
using WeBoard.Core.Enums;

namespace WeBoard.Core.Components.Base
{
    public abstract class InteractiveComponentBase : ComponentBase, IDraggable, IResizable
    {
        protected List<ResizeHandler> resizeHandles = new();
        public IEnumerable<ResizeHandler> GetResizeHandles() => resizeHandles;
        public float MinWidth => 100f;
        public float MinHeight => 100f;

        public virtual void Drag(Vector2f offset)
        {
            var bounds = GetGlobalBounds();
            foreach (var handle in resizeHandles)
            {
                handle.UpdatePosition(bounds);
            }
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            UpdateHandles();
            base.Draw(target, states);
            if (IsInFocus)
            {
                foreach (var handle in resizeHandles)
                    handle.Draw(target, states);
            }
        }

        public override void OnFocus()
        {
            base.OnFocus();
            if (resizeHandles.Count == 0)
            {
                foreach (ResizeDirectionEnum dir in Enum.GetValues(typeof(ResizeDirectionEnum)))
                {
                    var handle = new ResizeHandler(this, dir);
                    resizeHandles.Add(handle);
                }
            }
            
            UpdateHandles();
        }

        protected void UpdateHandles()
        {
            var bounds = GetGlobalBounds();
            foreach (var handle in resizeHandles)
            {
                handle.UpdatePosition(bounds);
            }
        }

        public virtual void Resize(Vector2f delta, ResizeDirectionEnum direction)
        {
            var originalSize = GetSize();
            var originalPosition = Position;

            var size = originalSize;
            var position = originalPosition;

            float minWidth = MinWidth;
            float minHeight = MinHeight;

            float allowedDeltaX = 0;
            float allowedDeltaY = 0;

            switch (direction)
            {
                case ResizeDirectionEnum.TopLeft:
                    allowedDeltaX = Math.Min(delta.X, size.X - minWidth);
                    allowedDeltaY = Math.Min(delta.Y, size.Y - minHeight);
                    size.X -= allowedDeltaX;
                    size.Y -= allowedDeltaY;
                    position.X += allowedDeltaX;
                    position.Y += allowedDeltaY;
                    break;

                case ResizeDirectionEnum.TopRight:
                    allowedDeltaX = Math.Max(delta.X, minWidth - size.X);
                    allowedDeltaY = Math.Min(delta.Y, size.Y - minHeight);
                    size.X += allowedDeltaX;
                    size.Y -= allowedDeltaY;
                    position.Y += allowedDeltaY;
                    break;

                case ResizeDirectionEnum.BottomLeft:
                    allowedDeltaX = Math.Min(delta.X, size.X - minWidth);
                    allowedDeltaY = Math.Max(delta.Y, minHeight - size.Y);
                    size.X -= allowedDeltaX;
                    size.Y += allowedDeltaY;
                    position.X += allowedDeltaX;
                    break;

                case ResizeDirectionEnum.BottomRight:
                    allowedDeltaX = Math.Max(delta.X, minWidth - size.X);
                    allowedDeltaY = Math.Max(delta.Y, minHeight - size.Y);
                    size.X += allowedDeltaX;
                    size.Y += allowedDeltaY;
                    break;
            }

            SetSize(size);
            Position = position;

            UpdateHandles();
            UpdateFocusShape();
        }

        public abstract Vector2f GetSize();
        public abstract void SetSize(Vector2f size);
    }
}
