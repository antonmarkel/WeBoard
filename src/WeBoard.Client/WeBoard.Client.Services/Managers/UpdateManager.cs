using WeBoard.Core.Components.Interfaces;
using WeBoard.Core.Updates.Creation;
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
                foreach (var update in trackable.Updates)
                {
                    TrackUpdate(update);
                }

                trackable.Updates = [];
            }
        }
        public void TrackUpdate(IUpdate update)
        {
            NetworkManager.GetInstance().SendUpdate(update);
            _updates.Add(update);
        }

        public void ApplyUpdate(IUpdate update)
        {
            if (update is CreateUpdate createUpdate)
            {
                _componentManager.AddComponent(createUpdate.GetComponent());
                return;
            }

            if (update is RemoveUpdate removeUpdate)
            {
                _componentManager.RemoveComponent(removeUpdate.TargetId);
                return;
            }

            var component = _componentManager.GetComponentsForLogic()
                .FirstOrDefault(comp => comp.Id == update.TargetId);

            if (component is ITrackable trackable)
                update.Apply(trackable);
        }

        public void RemoveLastUpdate()
        {
            if (_updates.Count == 0)
                return;

            var update = _updates.Last();
            _updates.Remove(update);

            var cancelUpdate = update.GetCancelUpdate();

            ApplyUpdate(cancelUpdate);
        }
    }
}
