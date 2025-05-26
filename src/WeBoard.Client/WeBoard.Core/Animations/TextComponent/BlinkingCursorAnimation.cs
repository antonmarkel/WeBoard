using WeBoard.Core.Animations.Base;

namespace WeBoard.Core.Animations.TextComponent
{
    public class BlinkingCursorAnimation : AnimationBase
    {
        public BlinkingCursorAnimation(float duration)
            : base(duration) { }

        protected override void ApplyAnimation(float progress)
        {
            if (_target is Components.Visuals.TextComponent text && text.IsEditing)
            {
                text.SetCursorVisible(progress < 0.5f);
            }
        }

        public override void Reset()
        {
            if (_target is Components.Visuals.TextComponent text)
                text.SetCursorVisible(true);
            base.Reset();
        }

        protected override void OnCompleted()
        {
            base.OnCompleted();
            Reset();
        }
    }
}
