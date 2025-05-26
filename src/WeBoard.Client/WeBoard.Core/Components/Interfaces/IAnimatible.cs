using System.Collections.Immutable;
using WeBoard.Core.Animations.Interfaces;

namespace WeBoard.Core.Components.Interfaces
{
    public interface IAnimatible
    {
        public IImmutableList<IAnimation> ActiveAnimations { get; }
        void PlayAnimation(IAnimation animation);
    }
}
