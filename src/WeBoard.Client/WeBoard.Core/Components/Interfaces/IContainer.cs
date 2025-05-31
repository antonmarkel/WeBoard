using System.Collections.Immutable;
using WeBoard.Core.Components.Base;

namespace WeBoard.Core.Components.Interfaces
{
    public interface IContainer
    {
        ImmutableList<MenuComponentBase> Children { get; }
    }
}
