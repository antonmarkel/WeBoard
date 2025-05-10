using SFML.Graphics;
using SFML.System;
using WeBoard.Core.Components.Base;
using WeBoard.Core.Components.Interfaces;

namespace WeBoard.Core.Components.Menu.Scrolls
{
    public class ScrollBarComponent : MenuComponentBase, IScrollable
    {
        public Shape TrackShape { get; set; }
        public RectangleShape ThumbShape { get; set; }

        private const  float MaxValue = 1f;
        private float _value = 0f;
        private float _step = 0.05f;
        public float Value { get => _value; }
        public float ThumbSize { 
            set {
                var fixedValue = Math.Clamp(value, 0.001f, 1f);
                ThumbShape.Size = new Vector2f(ThumbShape.Size.X, Height * fixedValue);
            } 
        }
        public float Step
        {
            get => _step;
            set => _step = Math.Clamp(value, 0.005f, 1f);
        }
        public float Height { get; }
        public readonly Vector2f _position;

        public ScrollBarComponent(Vector2f position, float height)
        {
            Height = height;
            _position = position;

            TrackShape = new RectangleShape(new Vector2f(5f, Height))
            {
                Position = position,
                FillColor = Color.Black,
            };
            ThumbShape = new RectangleShape(new Vector2f(5f,5f))
            {
                Position = position,
                FillColor = Color.White,
            };
        }

        public override FloatRect GetGlobalBounds() => TrackShape.GetGlobalBounds();
        public override FloatRect GetLocalBounds() => TrackShape.GetGlobalBounds();

        public override void Draw(RenderTarget target, RenderStates states)
        {
            if (IsVisible)
            {
                TrackShape.Draw(target, states);
                ThumbShape.Draw(target, states);
                base.Draw(target, states);
            }
        }

        public void Scroll(float delta)
        {
            if (delta == 0)
                return;
            _value += delta > 0 ? -Step : Step;
            _value = Math.Clamp(_value, 0f, 1f);
           
            ThumbShape.Position = new Vector2f(ThumbShape.Position.X, TrackShape.Position.Y + _value * (Height - ThumbShape.Size.Y));
           
        }
    }
}
