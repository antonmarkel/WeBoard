using System.Collections.Immutable;
using SFML.Graphics;
using SFML.System;
using WeBoard.Core.Animations.Interfaces;
using WeBoard.Core.Animations.TextComponent;
using WeBoard.Core.Components.Base;
using WeBoard.Core.Components.Handlers;
using WeBoard.Core.Components.Interfaces;

namespace WeBoard.Core.Components.Menu.Inputs
{
    public class TextInputComponent : MenuComponentBase, IAnimatible, ITextCursorControllable
    {
        private readonly Font _font;
        private readonly Text _text;
        private readonly RectangleShape _background;
        private readonly TextCursorHandler _cursorHandler;

        private readonly Vector2f _padding = new(4, 4);
        private readonly List<IAnimation> _activeAnimations = new();
        private BlinkingTextCursorAnimation? _cursorAnimation;

        private bool _isEditing = false;
        public bool IsEditing => _isEditing;
        private bool _cursorVisible = true;

        public event Action<string> OnInput;
        public event Action<char> OnInputKey;
        protected override Shape Shape => _background;

        public string Content
        {
            get => _text.DisplayedString;
            set
            {
                _text.DisplayedString = value;
                UpdateLayout();
            }
        }

        public IImmutableList<IAnimation> ActiveAnimations
        {
            get
            {
                _activeAnimations.RemoveAll(a => a.IsCompleted);
                return _activeAnimations.ToImmutableList();
            }
        }

        public uint FontSize
        {
            get => _text.CharacterSize;
            set
            {
                _text.CharacterSize = value;
                UpdateLayout();
            }
        }

        public override Vector2f Size
        {
            get => _background.Size;
            set
            {
                _background.Size = value;
                UpdateLayout();
            }
        }

        public override Vector2f Position
        {
            get => _background.Position;
            set
            {
                _background.Position = value;
                UpdateLayout();
            }
        }

        public override FloatRect GetScreenBounds() => _background.GetGlobalBounds();

        public TextInputComponent(Vector2f position, Vector2f size, uint fontSize = 24)
        {
            _font = new Font("C:/Windows/Fonts/arial.ttf");
            _text = new Text(string.Empty, _font, fontSize)
            {
                FillColor = Color.Black
            };

            _cursorHandler = new TextCursorHandler(_font, _padding);

            _background = new RectangleShape(size)
            {
                FillColor = new Color(255, 255, 255, 230),
                OutlineColor = Color.Black,
                OutlineThickness = 1
            };

            Position = position;
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            target.Draw(_background, states);

            if (_isEditing && _cursorVisible)
            {
                if (string.IsNullOrEmpty(_text.DisplayedString))
                {
                    _cursorHandler.DrawCenteredCursor(target, states, _background, _text.CharacterSize);
                }
                else
                {
                    _cursorHandler.DrawTextWithCursor(target, states, _text, _background);
                }
            }
            else
            {
                target.Draw(_text, states);
            }
        }

        private void UpdateLayout()
        {
            FloatRect bounds = _text.GetLocalBounds();
            float textWidth = bounds.Width;
            float textHeight = bounds.Height;
            float textTop = bounds.Top;

            float availableWidth = _background.Size.X - _padding.X * 2;

            if (textWidth <= availableWidth)
            {
                _text.Origin = new Vector2f(textWidth / 2f, textTop + textHeight / 2f);
                _text.Position = _background.Position + new Vector2f(_background.Size.X / 2f, _background.Size.Y / 2f);
            }
            else
            {
                _text.Origin = new Vector2f(0, textTop + textHeight / 2f);
                _text.Position = _background.Position + new Vector2f(_padding.X, _background.Size.Y / 2f);
            }
        }

        public void AppendChar(string ch)
        {
            var proposedText = _text.DisplayedString + ch;

            var testText = new Text(proposedText, _font, _text.CharacterSize);

            float maxWidth = _background.Size.X - _padding.X * 2;
            float proposedWidth = testText.GetLocalBounds().Width;

            if (proposedWidth > maxWidth)
                return;

            _text.DisplayedString = proposedText;
            OnInputKey?.Invoke(ch[0]);
            UpdateLayout();
            _cursorAnimation?.Reset();
        }


        public void Backspace()
        {
            if (_text.DisplayedString.Length > 0)
            {
                _text.DisplayedString = _text.DisplayedString[..^1];
                UpdateLayout();
                _cursorAnimation?.Reset();
            }
        }

        public void StartEditing()
        {
            _isEditing = true;
            _cursorVisible = true;

            _activeAnimations.RemoveAll(a => a is BlinkingTextCursorAnimation);
            _cursorAnimation = new BlinkingTextCursorAnimation(1000);
            _cursorAnimation.ApplyTo(this);
            _activeAnimations.Add(_cursorAnimation);
        }

        public void StopEditing()
        {
            _isEditing = false;
            _cursorVisible = false;

            _cursorAnimation = null;
            _activeAnimations.RemoveAll(a => a is BlinkingTextCursorAnimation);
            OnInput?.Invoke(Content);
        }
        public void PlayAnimation(IAnimation animation)
        {
            animation.ApplyTo(this);
            _activeAnimations.Add(animation);
        }

        public void SetCursorVisible(bool visible)
        {
            _cursorVisible = visible;
        }
        public override void OnClick(Vector2f offset)
        {
            if (!_isEditing)
                StartEditing();
            else
                StopEditing();
        }

        public override void OnFocus()
        {
            base.OnFocus();
            StartEditing();
        }

        public override void OnLostFocus()
        {
            base.OnLostFocus();
            StopEditing();
        }
    }
}
