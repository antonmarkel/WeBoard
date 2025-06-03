using System.Collections.Immutable;
using SFML.Graphics;
using SFML.System;
using WeBoard.Core.Animations.Interfaces;
using WeBoard.Core.Animations.TextComponent;
using WeBoard.Core.Components.Base;
using WeBoard.Core.Components.Handlers;
using WeBoard.Core.Components.Interfaces;
using WeBoard.Core.Network.Serializable.Interfaces;
using WeBoard.Core.Network.Serializable.Visuals;
using WeBoard.Core.Updates.Creation;

namespace WeBoard.Core.Components.Visuals
{
    public class TextComponent : InteractiveComponentBase, IAnimatible, ICleanable, ITextCursorControllable
    {
        private readonly TextCursorHandler _cursorHandler;
        private readonly TextLayoutHandler _layoutHandler;
        private readonly RectangleShape _focusRectangle;
        private readonly Text _text;
        private readonly Font _font;

        private bool _isEditing;
        private bool _cursorVisible = true;
        private bool _showTextCursor = false;

        private Vector2f _currentSize;
        private readonly Vector2f _padding = new(20, 5);
        private readonly List<IAnimation> _activeAnimations = new();
        private BlinkingTextCursorAnimation? _cursorAnimation;

        protected override Shape Shape => _focusRectangle;
        public bool ShouldBeClean { get; set; } = false;
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

            _layoutHandler.FitFontSizeToBounds(_text, newSize);
            _layoutHandler.UpdateTextOriginAndPosition(_text, _focusRectangle.Position);

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

            _cursorHandler = new TextCursorHandler(_font, _padding);
            _layoutHandler = new TextLayoutHandler(_font, _padding, MinWidth, MinHeight);

            _currentSize = new Vector2f(250, 80);
            SetSize(_currentSize);
            Position = position;

            StartEditing();
        }

        public void StartEditing()
        {
            _isEditing = true;
            _activeAnimations.RemoveAll(anim => anim is BlinkingTextCursorAnimation);

            _cursorAnimation = new BlinkingTextCursorAnimation(1000);
            _cursorAnimation.Reset();
            _cursorAnimation.ApplyTo(this);
            _activeAnimations.Add(_cursorAnimation);
        }

        public void StopEditing()
        {
            if (_isEditing)
            {
                var ser = ToSerializable();
                var newText = new TextComponent(new());
                newText.FromSerializable(ser);
                Updates.Add(new RemoveUpdate(Id, ser));
                Updates.Add(new CreateUpdate(newText.Id, newText.ToSerializable()));
            }

            _isEditing = false;
            _cursorVisible = false;
            _activeAnimations.RemoveAll(anim => anim is BlinkingTextCursorAnimation);
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

            if (string.IsNullOrWhiteSpace(Content))
                ShouldBeClean = true;
        }
        public override void Draw(RenderTarget target, RenderStates states)
        {
            target.Draw(_focusRectangle, states);

            if (_isEditing && _cursorVisible)
                _cursorHandler.Draw(target, states, _text, _focusRectangle);
            else
                target.Draw(_text, states);

            base.Draw(target, states);
        }

        private void CalculateSizeAndPosition()
        {
            _currentSize = _layoutHandler.CalculateSize(_text.DisplayedString, _text.CharacterSize);

            _focusRectangle.Size = _currentSize;
            _focusRectangle.Origin = _currentSize / 2f;

            _layoutHandler.UpdateTextOriginAndPosition(_text, _focusRectangle.Position);

            UpdateHandles();
            UpdateFocusShape();
        }

        private void UpdateTextPosition()
        {
            var bounds = _text.GetLocalBounds();

            _text.Origin = new Vector2f(
                bounds.Left + bounds.Width / 2f,
                bounds.Top + bounds.Height / 2f
            );

            _text.Position = _focusRectangle.Position;
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

        public override IBinarySerializable ToSerializable()
        {
            var serializable = new SerializableText();
            serializable.Position = Position;
            serializable.Id = Id;
            serializable.Rotation = Rotation;
            serializable.Size = GetSize();
            serializable.Text = Content;
            
            return serializable;
        }

        public override void FromSerializable(IBinarySerializable serializable)
        {
            if (serializable is SerializableText serializableText)
            {
                Position = serializableText.Position;
                Id = serializableText.Id;
                Rotation = serializableText.Rotation;
                SetSize(serializableText.Size);
                Content = serializableText.Text;
            }
        }
    }
}
