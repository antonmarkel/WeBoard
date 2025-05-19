using SFML.Graphics;
using WeBoard.Core.Animations.Base;
using WeBoard.Core.Components.Menu.Buttons;

namespace WeBoard.Core.Animations.ButtonAnimations
{
    public class ButtonClickAnimation : AnimationBase
    {
        private Color _startColor;
        private Color _endColor;

        public ButtonClickAnimation(Color startColor, Color endColor, float duration)
            : base(duration)
        {
            _startColor = startColor;
            _endColor = endColor;
        }

        protected override void ApplyAnimation(float progress)
        {
            if (_target is ButtonComponent button)
            {
                button.BackgroundColor = new Color(
                    (byte)(_startColor.R + (_endColor.R - _startColor.R) * progress),
                    (byte)(_startColor.G + (_endColor.G - _startColor.G) * progress),
                    (byte)(_startColor.B + (_endColor.B - _startColor.B) * progress),
                    (byte)(_startColor.A + (_endColor.A - _startColor.A) * progress)
                );
            }
        }

        public override void Reset()
        {
            if (_target is ButtonComponent button)
                button.BackgroundColor = _endColor;
            base.Reset();
        }
    }
}
