using System.Collections.Concurrent;
using System.Collections.Immutable;
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
        private int count = 0;
        public ConcurrentDictionary<Guid, ComponentBase> RenderObjects { get; set; } = [];

        public bool TryAddComponent(ComponentBase component)
        {
            var guid = Guid.NewGuid();
            component.ZIndex = count++;
            return RenderObjects.TryAdd(guid, component);
        }

        public IImmutableList<ComponentBase> GetComponents(bool forLogic)
        {
            lock (RenderObjects)
            {
                var query = RenderObjects.Values.AsQueryable();
                if (forLogic)
                    query = query.OrderByDescending(comp => comp.ZIndex);
                else
                    query = query.OrderBy(comp => comp.ZIndex);

                return query.ToImmutableList();
            }
        }
    }
}
