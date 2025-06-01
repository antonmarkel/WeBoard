using SFML.System;
using WeBoard.Core.Animations.Button;

namespace WeBoard.Core.Components.Menu.Buttons.RadioButtons
{
    public class RadioButtonComponent : ButtonComponent
    {
        protected bool _isChosen;
        public event Action<bool> OnGroupUpdate;
        private RadioButtonGroup _group;
        public RadioButtonGroup Group
        {
            get => _group;
            set
            {
                _group = value;
                _group.AddButton(this);
            }
        }
        public RadioButtonComponent(Vector2f position, Vector2f size) : base(position, size)
        {
        }

        public void OnGroupClick(bool self)
        {
            OnGroupUpdate?.Invoke(self);
            if (self)
            {
                if (!_isChosen)
                {
                    _activeAnimations.Remove(_resizeAnimation);
                    _resizeAnimation?.Reset();
                    _resizeAnimation = new ButtonResizeAnimation(Size, 1.15f, 100);
                    OutlineThickness += 1;
                    _resizeAnimation.ApplyTo(this);
                    _activeAnimations.Add(_resizeAnimation);
                }
                _isChosen = true;
                return;
            }

            if (_isChosen)
            {
                OutlineThickness -= 1;
                _activeAnimations.Remove(_resizeAnimation);
                _resizeAnimation.Reset();
            }

            _isChosen = false;
        }

        public override void OnClick(Vector2f offset)
        {
            base.OnClick(offset);
            _clickAnimation.Reset();
            _activeAnimations.Remove(_clickAnimation);

            Group.SetOption(this);
        }
        public override void OnMouseOver()
        {
            if (!_isChosen)
                base.OnMouseOver();
        }

        public override void OnMouseLeave()
        {
            if (!_isChosen)
                base.OnMouseLeave();
        }

    }
}
