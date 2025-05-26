using WeBoard.Core.Edit.Properties.Base;

namespace WeBoard.Core.Edit.Base
{
    public abstract class Edit<T> : EditBase
    {
        protected EditProperty<T> _property;
        public Edit(EditProperty<T> property) : base(typeof(T))
        {
            _property = property;
        }
    }
}
