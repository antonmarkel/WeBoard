using SFML.Graphics;
using SFML.System;
using WeBoard.Core.Components.Handlers;
using WeBoard.Core.Components.Interfaces;
using WeBoard.Core.Enums;
using WeBoard.Core.Updates.Interactive;
using WeBoard.Core.Updates.Interfaces;
using Color = SFML.Graphics.Color;

namespace WeBoard.Core.Components.Base
{
    public abstract class InteractiveComponentBase : ComponentBase, IDraggable, IResizable, IRotatable, ITrackable
    {
        protected List<ResizeHandler> resizeHandles = new();
        private RotateHandler? rotateHandle;
        public IEnumerable<ResizeHandler> GetResizeHandles() => resizeHandles;
        public RotateHandler? GetRotateHandle() => rotateHandle;
        public override FloatRect GetGlobalBounds() => Shape.GetGlobalBounds();
        public float MinWidth => 5f;
        public float MinHeight => 5f;
        public virtual Color FillColor
        {
            get => Shape.FillColor;
            set => Shape.FillColor = value;
        }

        public virtual Vector2f Position
        {
            get => Shape.Position;
            set => Shape.Position = value;
        }

        public virtual float Rotation
        {
            get => Shape.Rotation;
            set => Shape.Rotation = value;
        }

        public List<IUpdate> Updates { get; set; } = [];
        public bool IsUpdating { get; set; } = false;

        private Vector2f _startDraggingPosition = new();
        private bool _isDragging = false;
        private DateTime _lastDragAt = DateTime.UtcNow;

        private bool _isResizing = false;
        private Vector2f _startSize = new();

        private bool _isRotating = false;
        private float _startRotation;

        public void OnStartRotating()
        {
            if (!IsUpdating)
            {
                _isRotating = true;
                _startRotation = Rotation;
            }
        }

        public void OnStopRotating()
        {
            var wasRotating = _isRotating;
            _isRotating = false;

            if (wasRotating && !IsUpdating)
                TrackUpdate(new RotateUpdate(Id, Rotation - _startRotation));
        }

        public virtual void SetRotation(float angle)
        {
            if (!_isRotating)
                OnStartRotating();

            Rotation = angle;
            UpdateHandles();
            UpdateFocusShape();
        }

        public void OnStartDragging()
        {
            if (!IsUpdating)
            {
                _isDragging = true;
                _startDraggingPosition = Position;
            }

        }

        public void OnStopDragging()
        {

            if ((DateTime.UtcNow - _lastDragAt).TotalMilliseconds < 200 && IsInFocus)
                return;

            var wasDragging = _isDragging;
            _isDragging = false;

            if (wasDragging && !IsUpdating)
                TrackUpdate(new DragUpdate(Id, Position - _startDraggingPosition));
        }

        public virtual void Drag(Vector2f offset)
        {
            if (!_isDragging)
            {
                OnStartDragging();
            }

            _lastDragAt = DateTime.UtcNow;
            Position += offset;

            UpdateHandles();
            UpdateFocusShape();
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            target.Draw(Shape, states);
            UpdateHandles();
            base.Draw(target, states);
            if (IsInFocus)
            {
                foreach (var handle in resizeHandles)
                    handle.Draw(target, states);
                rotateHandle?.Draw(target, states);
            }
        }

        public override void OnLostFocus()
        {
            base.OnLostFocus();
            OnStopDragging();
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

            rotateHandle = rotateHandle ?? new RotateHandler(this);
            UpdateHandles();
        }

        protected void UpdateHandles()
        {
            rotateHandle?.UpdatePosition(GetGlobalBounds());

            Vector2f center = Position;
            Vector2f size = GetSize();
            float angle = Rotation;

            foreach (var handle in resizeHandles)
            {
                handle.UpdatePosition(center, size, angle);
            }

        }

        public virtual void Resize(Vector2f delta, ResizeDirectionEnum direction)
        {
            var originalSize = GetSize();

            Vector2f localOffset = direction switch
            {
                ResizeDirectionEnum.TopLeft => new Vector2f(-1, -1),
                ResizeDirectionEnum.TopRight => new Vector2f(1, -1),
                ResizeDirectionEnum.BottomLeft => new Vector2f(-1, 1),
                ResizeDirectionEnum.BottomRight => new Vector2f(1, 1),
                _ => new Vector2f(0, 0)
            };

            Vector2f newSize = new Vector2f(
                MathF.Max(MinWidth, originalSize.X + delta.X * localOffset.X),
                MathF.Max(MinHeight, originalSize.Y + delta.Y * localOffset.Y)
            );

            Vector2f sizeDiff = newSize - originalSize;
            Vector2f centerShift = new Vector2f(
                (sizeDiff.X / 2f) * localOffset.X,
                (sizeDiff.Y / 2f) * localOffset.Y
            );

            float angleRad = Rotation * MathF.PI / 180f;
            float cos = MathF.Cos(angleRad);
            float sin = MathF.Sin(angleRad);

            Vector2f rotatedShift = new Vector2f(
                centerShift.X * cos - centerShift.Y * sin,
                centerShift.X * sin + centerShift.Y * cos
            );

            SetSize(newSize);
            Position += rotatedShift;

            UpdateHandles();
            UpdateFocusShape();
        }


        public abstract Vector2f GetSize();

        public void OnStartResizing()
        {
            if (!IsUpdating)
            {
                _isResizing = true;
                _startSize = GetSize();
            }
        }

        public void OnStopResizing()
        {
            var wasResizing = _isResizing;
            _isResizing = false;

            if (wasResizing && !IsUpdating)
                TrackUpdate(new ResizeUpdate(Id, GetSize() - _startSize));
        }

        public virtual void SetSize(Vector2f size)
        {
            if (!_isResizing)
                OnStartResizing();
        }

        public void TrackUpdate(IUpdate update)
        {
            if (!IsUpdating)
                Updates.Add(update);
        }
    }
}
