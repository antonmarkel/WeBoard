using System.Collections.Immutable;
using SFML.System;
using WeBoard.Core.Collections;
using WeBoard.Core.Components.Base;
using WeBoard.Core.Components.Interfaces;

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
        private ZIndexComponentSortedSet _componentSet { get; set; } = new();
        private List<MenuComponentBase> _menuComponents { get; set; } = [];

        public ComponentBase? GetByScreenPoint(Vector2i point, out Vector2f offset)
        {
            Vector2f newOffset = new Vector2f(0, 0);
            var menuComponent = _menuComponents.FirstOrDefault(comp => comp.Intersect(point,out newOffset));
            if (menuComponent != null)
            {
                offset = newOffset;
                return menuComponent;
            }

            var component = _componentSet.GetComponentsAscending().FirstOrDefault(comp => comp.Intersect(point, out newOffset));
            offset = newOffset;

            return component;
        }

        public void InitMenu(IEnumerable<MenuComponentBase> components)
        {
            var tempList = new List<MenuComponentBase>();
            foreach (var component in components)
            {
                AddMenuComponent(component);
            }
        }

        public void AddMenuComponent(MenuComponentBase menuComponent)
        {
            lock (_menuComponents)
            {
                if (menuComponent is IContainer container)
                    container.Children.ForEach(comp => AddMenuComponent(comp));

                if (menuComponent is IAnimatible animatible)
                    AnimationManager.GetInstance().Add(animatible);

                _menuComponents.Add(menuComponent);
            }
          
        }
        public void RemoveMenuComponent(MenuComponentBase menuComponent)
        {
            _menuComponents.Remove(menuComponent);
        }

        public void AddComponent(ComponentBase component)
        {
            component.ZIndex = _componentSet.Last is null ? 0 : _componentSet.Last.ZIndex + 1;
            _componentSet.Add(component);
            if(component is IAnimatible animatible)
                AnimationManager.GetInstance().Add(animatible);
        }
        public void RemoveComponent(ComponentBase component)
        {
            _componentSet.Remove(component);
            if (component is IAnimatible animatible)
                AnimationManager.GetInstance().Remove(animatible);
        }

        public void RemoveComponent(int componentId)
        {
            var component = _componentSet.GetById(componentId);
            if (component is null)
                return;

            _componentSet.Remove(component);
            if (component is IAnimatible animatible)
                AnimationManager.GetInstance().Remove(animatible);
        }

        public IEnumerable<ComponentBase> GetComponentsForLogic()
        {
            
            return _menuComponents.Union(_componentSet.GetComponentsAscending());
        }

        public IEnumerable<MenuComponentBase> GetMenuComponents()
        {
            return _menuComponents;
        }

        public IEnumerable<ComponentBase> GetComponentsForRender()
        {
            return _componentSet.GetComponentsDescending();
        }
    }
}
