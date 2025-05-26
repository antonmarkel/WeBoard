using WeBoard.Core.Components.Base;
using WeBoard.Core.Components.Interfaces;
using WeBoard.Core.Edit.Properties.Base;

namespace WeBoard.Core.Edit.Base
{
    public abstract class EditBase<T> : MenuComponentBase
    {
        protected EditProperty<T> _property;

        public EditBase(EditProperty<T> property)
        {
            _property = property;
        }
    }
}
