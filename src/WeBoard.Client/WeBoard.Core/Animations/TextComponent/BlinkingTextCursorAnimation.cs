using WeBoard.Core.Animations.Base;
using WeBoard.Core.Components.Interfaces;

namespace WeBoard.Core.Animations.TextComponent
{
    public class BlinkingTextCursorAnimation : AnimationBase
    {
        public BlinkingTextCursorAnimation(float duration)
            : base(duration) { }

        protected override void ApplyAnimation(float progress)
        {
            if (_target is ITextCursorControllable cursor && cursor.IsEditing)
            {
                cursor.SetCursorVisible(progress < 0.5f);
            }
        }

        public override void Reset()
        {
            if (_target is ITextCursorControllable cursor)
                cursor.SetCursorVisible(true);

            base.Reset();
        }

        protected override void OnCompleted()
        {
            base.OnCompleted();
            Reset();
        }
    }
}