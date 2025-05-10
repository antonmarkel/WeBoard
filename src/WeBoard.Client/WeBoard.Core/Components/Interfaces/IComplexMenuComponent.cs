using SFML.System;
using System.Numerics;
using WeBoard.Core.Components.Base;

namespace WeBoard.Core.Components.Interfaces
{
    public interface IComplexMenuComponent
    {
        public MenuComponentBase? ChildUnderMouse(Vector2f offset, out Vector2f inOffset);
    }
}
