using System.Collections.Immutable;
using WeBoard.Core.Edit.Properties.Base;

namespace WeBoard.Core.Components.Interfaces
{
    public interface IEditable
    {
        public IImmutableList<EditPropertyBase> EditProperties { get; }
    }
}
