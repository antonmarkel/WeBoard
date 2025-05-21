using SFML.Graphics;
using SFML.System;
using WeBoard.Core.Components.Interfaces;

namespace WeBoard.Core.Components.Base
{
    public abstract class MenuComponentBase : ComponentBase, IClickable, IHidden
    {
        public bool IsHidden { get; set; }
        private Vector2f _beforeHiddenPosition = new();

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

        public virtual void Show()
        {
            if(IsHidden)
                Position = _beforeHiddenPosition;

            IsHidden = false;
        }
        public virtual void Hide()
        {
            if (IsHidden)
                return;

            _beforeHiddenPosition = Position;
            Position = new Vector2f(-10000, -10000);
            IsHidden = true;
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            if(!IsHidden) 
                base.Draw(target, states);
        }
    }
}
