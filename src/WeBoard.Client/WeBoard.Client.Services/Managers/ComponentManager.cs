using SFML.System;
using WeBoard.Core.Collections;
using WeBoard.Core.Components.Base;

namespace WeBoard.Client.Services.Managers
{
    public class ComponentManager
    {
        private static ComponentManager? Instance;
        public ComponentManager() { }
        public static ComponentManager GetInstance()
        {
            return Instance ?? (Instance = new());
        }
        public int Count { get => _componentSet.Count; }
        private ZIndexComponentSortedSet<ComponentBase> _componentSet { get; set; } = new();

        public void AddComponent(ComponentBase component)
        {
            component.ZIndex = _componentSet.Last is null ? 0 : _componentSet.Last.ZIndex + 1;
            _componentSet.Add(component);
        }
        public void RemoveComponent(ComponentBase component)
        {
            _componentSet.Remove(component);

        }

        public ComponentBase? GetByPoints(Vector2f worldPoint, Vector2i screenPoint)
        {
            IEnumerable<MenuComponentBase> menuComponents = MenuManager.GetInstance().GetMenuComponents();
            var clickedComponent = (ComponentBase?)menuComponents.FirstOrDefault(obj => obj.Intersect(screenPoint, out _));
            if (clickedComponent is null)
            {
                var pointInt = new Vector2i((int)worldPoint.X, (int)worldPoint.Y);
                var components = GetComponentsForLogic();
                clickedComponent = components.FirstOrDefault(obj => obj.Intersect(pointInt, out _));
            }

            return clickedComponent;

        }

        public IEnumerable<ComponentBase> GetComponentsForLogic()
        {
            return _componentSet.GetComponentsAscending();
        }

        public IEnumerable<ComponentBase> GetComponentsForRender()
        {
            return _componentSet.GetComponentsDescending();
        }
    }
}
