using WeBoard.Core.Components.Interfaces;

namespace WeBoard.Client.Services.Managers
{
    public class AnimationManager
    {
        private static AnimationManager? Instance;
        private AnimationManager() { }
        public static AnimationManager GetInstance()
        {
            return Instance ?? (Instance = new());
        }

        private List<IAnimatible> _animComponents = [];
        public void Add(IAnimatible animComponent) => _animComponents.Add(animComponent);
        public void Remove(IAnimatible animComponent) => _animComponents.Remove(animComponent);
        public void OnUpdate(float deltaTime)
        {
            foreach (var animComponent in _animComponents)
            {
                var activeAnimations = animComponent.ActiveAnimations;
                foreach (var anim in activeAnimations)
                {

                    anim.Update(deltaTime);
                }
            }
        }
    }
}
