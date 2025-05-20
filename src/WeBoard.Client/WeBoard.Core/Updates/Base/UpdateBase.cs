using WeBoard.Core.Components.Interfaces;
using WeBoard.Core.Updates.Interfaces;

namespace WeBoard.Core.Updates.Base
{
    public abstract class UpdateBase : IUpdate
    {
        public long TargetId { get;}
        public DateTime Date { get; }
        private Action<ITrackable> UpdateAction => UpdateActionMethod;
        public UpdateBase(long id)
        {
            TargetId = id;
            Date = DateTime.UtcNow;

        }

        public abstract void UpdateActionMethod(ITrackable trackable);
        public void Apply(ITrackable trackable)
        {
            trackable.IsUpdating = true;
            UpdateAction(trackable);
            trackable.IsUpdating = false;
        }
        public abstract IUpdate GetCancelUpdate();

    }
}
