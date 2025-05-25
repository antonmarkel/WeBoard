namespace WeBoard.Core.Components.Handlers
{
    using SFML.Graphics;
    using SFML.System;

    namespace WeBoard.Core.Components.Handlers
    {
        public class TextCursorHandler
        {
            private readonly Font _font;
            private readonly Vector2f _padding;

            public TextCursorHandler(Font font, Vector2f padding)
            {
                _font = font;
                _padding = padding;
            }

            public void Draw(RenderTarget target, RenderStates states, Text text, RectangleShape focusRect)
            {
                string displayText = text.DisplayedString;

                if (string.IsNullOrWhiteSpace(displayText))
                    DrawPlaceholderWithCursor(target, states, focusRect, text.CharacterSize);
                else
                    DrawTextWithCursor(target, states, text, focusRect);
            }
            private void DrawPlaceholderWithCursor(RenderTarget target, RenderStates states, RectangleShape rect, uint fontSize)
            {
                var baseX = _padding.X;
                var centerY = rect.Size.Y / 2f;

                var cursor = new Text("| ", _font, fontSize)
                {
                    FillColor = Color.Black
                };
                var cb = cursor.GetLocalBounds();
                cursor.Origin = new Vector2f(0, cb.Top + cb.Height / 2f);
                cursor.Position = new Vector2f(baseX, centerY);

                var placeholder = new Text("Type", _font, fontSize)
                {
                    FillColor = new Color(150, 150, 150)
                };
                var pb = placeholder.GetLocalBounds();
                placeholder.Origin = new Vector2f(0, pb.Top + pb.Height / 2f);
                placeholder.Position = new Vector2f(baseX + cursor.GetGlobalBounds().Width, centerY);

                var transform = rect.Transform;

                states.Transform *= transform;
                target.Draw(cursor, states);
                target.Draw(placeholder, states);
            }

            private void DrawTextWithCursor(RenderTarget target, RenderStates states, Text text, RectangleShape rect)
            {
                target.Draw(text, states);

                var cursor = new Text("|", _font, text.CharacterSize)
                {
                    FillColor = Color.Black,
                    Rotation = text.Rotation
                };
                var cb = cursor.GetLocalBounds();
                cursor.Origin = new Vector2f(0, cb.Top);

                Vector2f localCharPos;
                {
                    var oldOrigin = text.Origin;
                    var oldPosition = text.Position;

                    text.Origin = new Vector2f(0, 0);
                    text.Position = new Vector2f(0, 0);
                    localCharPos = text.FindCharacterPos((uint)text.DisplayedString.Length);
                    text.Origin = oldOrigin;
                    text.Position = oldPosition;
                }

                float verticalOffset = text.CharacterSize * 0.25f; 
                localCharPos.Y += verticalOffset;

                var globalCharPos = text.Transform.TransformPoint(localCharPos);
                cursor.Position = globalCharPos;

                target.Draw(cursor, states);

            }
        }
    }
}
