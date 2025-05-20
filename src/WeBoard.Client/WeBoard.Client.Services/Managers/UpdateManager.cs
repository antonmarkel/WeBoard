using WeBoard.Core.Components.Interfaces;
using WeBoard.Core.Updates.Interfaces;

namespace WeBoard.Client.Services.Managers
{
    public class UpdateManager
    {
        private static UpdateManager? Instance;
        private ComponentManager _componentManager = ComponentManager.GetInstance();
        public UpdateManager() { }
        public static UpdateManager GetInstance()
        {
            return Instance ?? (Instance = new());
        }

        private List<IUpdate> _updates = [];

        public void CollectUpdates()
        {
            var components = _componentManager.GetComponentsForLogic().Where(comp => comp is ITrackable);

            foreach (var component in components)
            {
                var trackable = (ITrackable)component;
                _updates.AddRange(trackable.Updates);

                trackable.Updates = [];
            }
        }

        public void RemoveLastUpdate()
        {
            if (_updates.Count == 0)
                return;

            var update = _updates.Last();
            _updates.Remove(update);

            var cancelUpdate = update.GetCancelUpdate();
            var component = _componentManager.GetComponentsForLogic()
                .FirstOrDefault(comp => comp.Id == cancelUpdate.TargetId);

            if(component is ITrackable trackable)
                cancelUpdate.Apply(trackable);
        }
    }
}
