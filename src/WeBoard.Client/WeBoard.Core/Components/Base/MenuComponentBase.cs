using SFML.Graphics;
using SFML.System;
using WeBoard.Core.Components.Interfaces;

namespace WeBoard.Core.Components.Base
{
    public abstract class MenuComponentBase : ComponentBase,IClickable
    {
        public override FloatRect GetGlobalBounds()
        {
            return GetScreenBounds();
        }

        public abstract FloatRect GetScreenBounds();

        public override float GetTotalArea()
        {
            return GetScreenBounds().Height * GetScreenBounds().Width;
        }

        public override bool Intersect(Vector2i screenPoint, out Vector2f offset)
        {
            var bounds = GetScreenBounds();
            offset = bounds.Position - new Vector2f(screenPoint.X, screenPoint.Y);

            return bounds.Contains(screenPoint.X, screenPoint.Y);
        }
        public abstract void OnClick(Vector2f offset);


        public override void OnFocus()
        {
            IsInFocus = true;
        }

        public override void OnLostFocus()
        {
            IsInFocus = false;
        }

        public override void OnMouseLeave()
        {
           
        }

        public override void OnMouseOver()
        {
     
        }
    }
}
