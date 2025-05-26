using SFML.Graphics;
using SFML.System;

namespace WeBoard.Core.Components.Handlers
{
    public class TextLayoutHandler
    {
        private readonly Font _font;
        private readonly Vector2f _padding;
        private readonly float _minWidth;
        private readonly float _minHeight;

        public TextLayoutHandler(Font font, Vector2f padding, float minWidth, float minHeight)
        {
            _font = font;
            _padding = padding;
            _minWidth = minWidth;
            _minHeight = minHeight;
        }

        public Vector2f CalculateSize(string content, uint characterSize)
        {
            string textForBounds = string.IsNullOrWhiteSpace(content) ? "Type" : content;

            var measureText = new Text(textForBounds, _font, characterSize);
            var bounds = measureText.GetLocalBounds();

            float width = bounds.Width + _padding.X * 2 + 10;
            float height = bounds.Height + _padding.Y * 2 + 10;

            return new Vector2f(
                MathF.Max(width, _minWidth),
                MathF.Max(height, _minHeight)
            );
        }

        public void UpdateTextOriginAndPosition(Text text, Vector2f containerPosition)
        {
            var bounds = text.GetLocalBounds();
            text.Origin = new Vector2f(
                bounds.Left + bounds.Width / 2f,
                bounds.Top + bounds.Height / 2f
            );
            text.Position = containerPosition;
        }

        public void FitFontSizeToBounds(Text text, Vector2f availableSize)
        {
            uint fontSize = 72;
            text.CharacterSize = fontSize;

            FloatRect bounds;
            do
            {
                text.CharacterSize = fontSize--;
                bounds = text.GetLocalBounds();
            }
            while ((bounds.Height + _padding.Y > availableSize.Y ||
                    bounds.Width + _padding.X > availableSize.X) && fontSize > 8);
        }
    }
}


