using SFML.Graphics;
using SFML.System;
using WeBoard.Core.Components.Handlers;
using WeBoard.Core.Components.Interfaces;
using WeBoard.Core.Enums;

namespace WeBoard.Core.Components.Base
{
    public abstract class InteractiveComponentBase : ComponentBase, IDraggable, IResizable, IRotatable
    {
        protected List<ResizeHandler> resizeHandles = new();
        private RotateHandler? rotateHandle;
        public IEnumerable<ResizeHandler> GetResizeHandles() => resizeHandles;
        public RotateHandler? GetRotateHandle() => rotateHandle;
        public override FloatRect GetGlobalBounds() => Shape.GetGlobalBounds();
        public float MinWidth => 100f;
        public float MinHeight => 100f;
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
        public virtual void SetRotation(float angle)
        {
            Rotation = angle;
            UpdateHandles();
            UpdateFocusShape();
        }

        public virtual void Drag(Vector2f offset)
        {
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

            if (this is IRotatable && this is InteractiveComponentBase component)
            {
                rotateHandle = new RotateHandler(component);
            }

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
            var originalPos = Position;

            // Определяем направление смещения (локальные единицы от центра)
            Vector2f localOffset = direction switch
            {
                ResizeDirectionEnum.TopLeft => new Vector2f(-1, -1),
                ResizeDirectionEnum.TopRight => new Vector2f(1, -1),
                ResizeDirectionEnum.BottomLeft => new Vector2f(-1, 1),
                ResizeDirectionEnum.BottomRight => new Vector2f(1, 1),
                _ => new Vector2f(0, 0)
            };

            // Вычисляем новое значение размера
            Vector2f newSize = new Vector2f(
                MathF.Max(MinWidth, originalSize.X + delta.X * localOffset.X),
                MathF.Max(MinHeight, originalSize.Y + delta.Y * localOffset.Y)
            );

            // Сдвигаем центр в зависимости от того, какая ручка тянется
            Vector2f sizeDiff = newSize - originalSize;
            Vector2f centerShift = new Vector2f(
                (sizeDiff.X / 2f) * localOffset.X,
                (sizeDiff.Y / 2f) * localOffset.Y
            );

            // Учитываем поворот при смещении центра
            float angleRad = Rotation * MathF.PI / 180f;
            float cos = MathF.Cos(angleRad);
            float sin = MathF.Sin(angleRad);

            Vector2f rotatedShift = new Vector2f(
                centerShift.X * cos - centerShift.Y * sin,
                centerShift.X * sin + centerShift.Y * cos
            );

            // Применяем изменения
            SetSize(newSize);
            Position += rotatedShift;

            UpdateHandles();
            UpdateFocusShape();
        }


        public abstract Vector2f GetSize();
        public abstract void SetSize(Vector2f size);
    }
}
