
using WeBoard.Core.Components.Interfaces;

namespace WeBoard.Core.Animations.Interfaces
{
    public interface IAnimation
    {
        void Update(float deltaTime);
        bool IsCompleted { get; }
        void Reset();
        void ApplyTo(IAnimatible target);
    }
}
