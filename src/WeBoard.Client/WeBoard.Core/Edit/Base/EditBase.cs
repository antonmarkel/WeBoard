using WeBoard.Core.Components.Base;

namespace WeBoard.Core.Edit.Base
{
    public abstract class EditBase : MenuComponentBase
    {
        public EditBase(Type editType)
        {
            EditType = editType;
        }

        public Type EditType { get; }
    }
}
