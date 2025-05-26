
using WeBoard.Core.Animations.Interfaces;
using WeBoard.Core.Components.Interfaces;

namespace WeBoard.Core.Animations.Base;

public abstract class AnimationBase : IAnimation
{
    protected float _duration;
    protected float _elapsedTime;
    protected IAnimatible _target;

    public bool IsCompleted { get; protected set; }

    protected AnimationBase(float duration)
    {
        _duration = duration;
    }

    public virtual void ApplyTo(IAnimatible target)
    {
        _target = target ?? throw new ArgumentNullException(nameof(target));
        Reset();
    }

    public virtual void Reset()
    {
        _elapsedTime = 0f;
        IsCompleted = false;
    }

    public virtual void Update(float deltaTime)
    {
        if (IsCompleted) return;

        _elapsedTime += deltaTime;
        var progress = Math.Clamp(_elapsedTime / _duration, 0f, 1f);

        ApplyAnimation(progress);

        if (_elapsedTime >= _duration)
        {
            IsCompleted = true;
            OnCompleted();
        }
    }

    protected abstract void ApplyAnimation(float progress);
    protected virtual void OnCompleted() { }
}

