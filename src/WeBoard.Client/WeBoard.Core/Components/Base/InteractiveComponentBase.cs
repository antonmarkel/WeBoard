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
            var originalPos = Position;

            Vector2f sizeDelta = delta;
            Vector2f newSize = originalSize;
            Vector2f newPosition = originalPos;

            switch (direction)
            {
                case ResizeDirectionEnum.TopLeft:
                    newSize.X -= sizeDelta.X;
                    newSize.Y -= sizeDelta.Y;
                    newPosition.X += sizeDelta.X;
                    newPosition.Y += sizeDelta.Y;
                    break;

                case ResizeDirectionEnum.TopRight:
                    newSize.X += sizeDelta.X;
                    newSize.Y -= sizeDelta.Y;
                    newPosition.Y += sizeDelta.Y;
                    break;

                case ResizeDirectionEnum.BottomLeft:
                    newSize.X -= sizeDelta.X;
                    newSize.Y += sizeDelta.Y;
                    newPosition.X += sizeDelta.X;
                    break;

                case ResizeDirectionEnum.BottomRight:
                    newSize.X += sizeDelta.X;
                    newSize.Y += sizeDelta.Y;
                    break;
            }

            if (newSize.X < MinWidth)
            {
                float diff = MinWidth - newSize.X;
                newSize.X = MinWidth;
                if (direction == ResizeDirectionEnum.TopLeft ||
                    direction == ResizeDirectionEnum.BottomLeft)
                    newPosition.X -= diff;
            }

            if (newSize.Y < MinHeight)
            {
                float diff = MinHeight - newSize.Y;
                newSize.Y = MinHeight;
                if (direction == ResizeDirectionEnum.TopLeft ||
                    direction == ResizeDirectionEnum.TopRight)
                    newPosition.Y -= diff;
            }

            SetSize(newSize);
            Position = newPosition;
            UpdateHandles();
            UpdateFocusShape();
        }

        public abstract Vector2f GetSize();
        public abstract void SetSize(Vector2f size);
    }
}
