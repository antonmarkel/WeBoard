using WeBoard.Core.Collections;
using WeBoard.Core.Components.Base;

namespace WeBoard.Client.Services.Managers
{
    public class MenuManager
    {
        private static MenuManager? Instance;
        public MenuManager() { }
        public static MenuManager GetInstance()
        {
            return Instance ?? (Instance = new());
        }
        private readonly ZIndexComponentSortedSet<MenuComponentBase> _sortedSet = new();

        public IEnumerable<MenuComponentBase> GetMenuComponents() => _sortedSet.GetComponentsAscending();

        public void Init(IEnumerable<MenuComponentBase> menuComponents)
        {
            foreach(var component in menuComponents)
            {
                _sortedSet.Add(component);
            }
        }

        public void Hide(MenuComponentBase menuComponent) => menuComponent.Hide();
        public void Show(MenuComponentBase menuComponent) => menuComponent.Show();
    }
}
