using System.Collections.Immutable;
using SFML.Graphics;
using SFML.System;
using WeBoard.Core.Animations.Interfaces;
using WeBoard.Core.Animations.TextComponent;
using WeBoard.Core.Components.Base;
using WeBoard.Core.Components.Interfaces;

namespace WeBoard.Core.Components.Visuals
{
    public class TextComponent : InteractiveComponentBase, IAnimatible
    {
        private readonly RectangleShape _focusRectangle;
        private readonly Text _text;
        private readonly Font _font;

        private bool _isEditing;
        private bool _cursorVisible = true;
        private bool _showTextCursor = false;

        private Vector2f _currentSize;
        private readonly Vector2f _padding = new(20, 5);
        private readonly List<IAnimation> _activeAnimations = new();
        private BlinkingCursorAnimation? _cursorAnimation;

        protected override Shape Shape => _focusRectangle;

        public bool IsEditing => _isEditing;

        public IImmutableList<IAnimation> ActiveAnimations
        {
            get
            {
                _activeAnimations.RemoveAll(a => a.IsCompleted);
                return _activeAnimations.ToImmutableList();
            }
        }

        public string Content
        {
            get => _text.DisplayedString;
            set
            {
                _text.DisplayedString = value;
                CalculateSizeAndPosition();
            }
        }

        public override Vector2f Position
        {
            get => _focusRectangle.Position;
            set
            {
                _focusRectangle.Position = value;
                UpdateTextPosition();
            }
        }

        public override float Rotation
        {
            get => _focusRectangle.Rotation;
            set
            {
                _focusRectangle.Rotation = value;
                _text.Rotation = value;
                UpdateHandles();
            }
        }

        public override Vector2f GetSize() => _focusRectangle.Size;

        public override void SetSize(Vector2f size)
        {
            var newSize = new Vector2f(
                MathF.Max(size.X, MinWidth),
                MathF.Max(size.Y, MinHeight));

            _focusRectangle.Size = newSize;
            _focusRectangle.Origin = newSize / 2f;

            UpdateFontSizeToFit(newSize);
            UpdateTextPosition();
            UpdateHandles();
            UpdateFocusShape();
        }

        public TextComponent(Vector2f position)
        {
            _font = new Font("C:/Windows/Fonts/arial.ttf");
            _text = new Text(string.Empty, _font, 24)
            {
                FillColor = Color.Black,
            };

            _focusRectangle = new RectangleShape
            {
                FillColor = Color.Transparent
            };

            _currentSize = new Vector2f(250, 60);
            SetSize(_currentSize);
            Position = position;

            StartEditing();
        }

        public void StartEditing()
        {
            _isEditing = true;
            _activeAnimations.RemoveAll(anim => anim is BlinkingCursorAnimation);

            _cursorAnimation = new BlinkingCursorAnimation(1000);
            _cursorAnimation.Reset();
            _cursorAnimation.ApplyTo(this);
            _activeAnimations.Add(_cursorAnimation);
        }

        public void StopEditing()
        {
            _isEditing = false;
            _cursorVisible = false;
            _activeAnimations.RemoveAll(anim => anim is BlinkingCursorAnimation);
            _cursorAnimation = null;
        }

        public void SetCursorVisible(bool visible)
        {
            _cursorVisible = visible;
        }

        public void PlayAnimation(IAnimation animation)
        {
            animation.ApplyTo(this);
            _activeAnimations.Add(animation);
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
        public override void Draw(RenderTarget target, RenderStates states)
        {
            target.Draw(_focusRectangle, states);

            if (_isEditing && _cursorVisible)
            {
                string displayText = _text.DisplayedString;
                if (string.IsNullOrWhiteSpace(displayText))
                {
                    var cursor = new Text("| ", _font, _text.CharacterSize)
                    {
                        FillColor = Color.Black
                    };

                    var cursorBounds = cursor.GetLocalBounds();
                    cursor.Origin = new Vector2f(0, cursorBounds.Top);
                    cursor.Position = new Vector2f(
                        _focusRectangle.Position.X - _focusRectangle.Size.X / 2f + _padding.X,
                        _focusRectangle.Position.Y - cursorBounds.Height / 2f);

                    var placeholder = new Text("Type", _font, _text.CharacterSize)
                    {
                        FillColor = new Color(150, 150, 150)
                    };

                    var placeholderBounds = placeholder.GetLocalBounds();
                    placeholder.Origin = new Vector2f(0, placeholderBounds.Top);
                    placeholder.Position = new Vector2f(
                        cursor.Position.X + cursor.GetGlobalBounds().Width,
                        cursor.Position.Y);

                    target.Draw(cursor, states);
                    target.Draw(placeholder, states);
                }
                else
                {
                    var tempText = new Text(_text)
                    {
                        DisplayedString = displayText + "|",
                        FillColor = Color.Black
                    };

                    var bounds = tempText.GetLocalBounds();
                    tempText.Origin = new Vector2f(0, bounds.Top);
                    tempText.Position = new Vector2f(
                        _focusRectangle.Position.X - _focusRectangle.Size.X / 2f + _padding.X,
                        _focusRectangle.Position.Y - bounds.Height / 2f);

                    target.Draw(tempText, states);
                }
            }
            else
            {
                target.Draw(_text, states);
            }

            base.Draw(target, states);
        }



        private void CalculateSizeAndPosition()
        {
            var bounds = _text.GetLocalBounds();

            var calculatedSize = new Vector2f(
                bounds.Width + _padding.X * 2 + 10,
                bounds.Height + _padding.Y * 2 + 10
            );

            _currentSize = new Vector2f(
                calculatedSize.X,
                calculatedSize.Y
            );


            var newSize = new Vector2f(
                MathF.Max(_currentSize.X, MinWidth),
                MathF.Max(_currentSize.Y, MinHeight)
            );

            _focusRectangle.Size = newSize;
            _focusRectangle.Origin = newSize / 2f;

            _text.Origin = new Vector2f(0, 0);

            UpdateTextPosition();
            UpdateHandles();
            UpdateFocusShape();
        }

        private void UpdateFontSizeToFit(Vector2f availableSize)
        {
            uint fontSize = 72;
            _text.CharacterSize = fontSize;

            FloatRect bounds;
            do
            {
                _text.CharacterSize = fontSize--;
                bounds = _text.GetLocalBounds();
            }
            while ((bounds.Height + _padding.Y > availableSize.Y ||
                    bounds.Width + _padding.X > availableSize.X) && fontSize > 8);
        }

        private void UpdateTextPosition()
        {
            var bounds = _text.GetLocalBounds();
            _text.Origin = new Vector2f(0, bounds.Top);
            _text.Position = new Vector2f(
                _focusRectangle.Position.X - _focusRectangle.Size.X / 2f + _padding.X,
                _focusRectangle.Position.Y - bounds.Height / 2f);
        }

        public void AppendChar(string ch)
        {
            _text.DisplayedString += ch;
            CalculateSizeAndPosition();
            _cursorAnimation?.Reset();
        }

        public void Backspace()
        {
            if (_text.DisplayedString.Length > 0)
            {
                _text.DisplayedString = _text.DisplayedString[..^1];
                CalculateSizeAndPosition();
                _cursorAnimation?.Reset();
            }
        }
    }
}
