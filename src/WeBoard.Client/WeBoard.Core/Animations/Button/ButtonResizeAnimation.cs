using SFML.System;
using WeBoard.Core.Animations.Base;
using WeBoard.Core.Components.Menu.Buttons;

namespace WeBoard.Core.Animations.Button
{
    public class ButtonResizeAnimation : AnimationBase
    {
        private readonly Vector2f _startSize, _endSize;

        public ButtonResizeAnimation(Vector2f startSize, Vector2f endSize,float duration)
            : base(duration)
        {
            _startSize = startSize;
            _endSize = endSize;
        }
        public ButtonResizeAnimation(Vector2f startSize, float deltaSize, float duration)
            : base(duration)
        {
            _startSize = startSize;
            _endSize = new Vector2f(_startSize.X * deltaSize,_startSize.Y * deltaSize);
        }


        protected override void ApplyAnimation(float progress)
        {
            if (_target is ButtonComponent button)
            {
                button.Size = new Vector2f(
                    _startSize.X + (_endSize.X - _startSize.X) * progress,
                    _startSize.Y + (_endSize.Y - _startSize.Y) * progress);
            }
        }

        public override void Reset()
        {
            if (_target is ButtonComponent button)
                button.Size = _startSize;
            base.Reset();
        }
    }
}
