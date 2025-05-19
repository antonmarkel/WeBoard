using System.Collections.Immutable;
using SFML.Graphics;
using SFML.System;
using WeBoard.Core.Animations.ButtonAnimations;
using WeBoard.Core.Animations.Interfaces;
using WeBoard.Core.Components.Base;
using WeBoard.Core.Components.Interfaces;
using WeBoard.Core.Drawables.Shapes;

namespace WeBoard.Core.Components.Menu.Buttons
{
    public class ButtonComponent : MenuComponentBase, IContainerView, IAnimatible
    {
        private RoundedRectangle _buttonShape;
        private RectangleShape _focusRectangle;
        private ButtonClickAnimation _clickAnimation;
        private List<IAnimation> _activeAnimations = [];
        public IContentView? ContentView { get; set; }
        public uint Padding { get; set; }
        public event Action<Vector2f> OnClickEvent;

        public override Vector2f Position
        {
            get => _buttonShape.Position;
            set => _buttonShape.Position = _focusRectangle.Position = value;
        }
        public Vector2f Size
        {
            get => _buttonShape.Size;
            set => _buttonShape.Size = _focusRectangle.Size = value;
        }
        public Color OutlineColor
        {
            get => _buttonShape.OutlineColor;
            set => _buttonShape.OutlineColor = value;
        }
        public float OutlineThickness
        {
            get => _buttonShape.OutlineThickness;
            set => _buttonShape.OutlineThickness = value;
        }
        public Color BackgroundColor
        {
            get => _buttonShape.FillColor;
            set => _buttonShape.FillColor = value;
        }

        protected override Shape Shape => _focusRectangle;

        public IImmutableList<IAnimation> ActiveAnimations
        {
            get
            {
                _activeAnimations = _activeAnimations.Where(anim => !anim.IsCompleted).ToList();

                return _activeAnimations.ToImmutableList();
            }
        }


        public ButtonComponent(Vector2f position, Vector2f size)
        {
            _buttonShape = new RoundedRectangle();
            _focusRectangle = new RectangleShape();

            Position = position;
            Size = size;
            OutlineThickness = 2;
            OutlineColor = Color.Black;
            BackgroundColor = new Color(0, 0, 0, 120);
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            UpdateContentView();

            _buttonShape.Draw(target, states);
            ContentView?.Draw(target, states);
        }

        private void UpdateContentView()
        {
            if (ContentView is not null)
            {
                ContentView.Size = Size - new Vector2f(Padding, Padding);
                ContentView.Position = Position + new Vector2f(Padding, Padding);
            }
        }

        public override FloatRect GetScreenBounds()
        {
            return _focusRectangle.GetGlobalBounds();
        }

        public override void OnLostFocus()
        {
            base.OnLostFocus();
        }

        public override void OnFocus()
        {
      
            base.OnLostFocus();
        }

        public override void OnClick(Vector2f offset)
        {
            _activeAnimations.Remove(_clickAnimation);
            _clickAnimation = new ButtonClickAnimation(Color.White, BackgroundColor, 200);
            _clickAnimation.Reset();
            _clickAnimation.ApplyTo(this);
            _activeAnimations.Add(_clickAnimation);

            OnClickEvent?.Invoke(offset);
            OnFocus();
        }

        public void PlayAnimation(IAnimation animation)
        {
            animation.ApplyTo(this);
            _activeAnimations.Add(animation);
        }
    }
}
