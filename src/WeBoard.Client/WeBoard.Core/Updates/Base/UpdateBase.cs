using WeBoard.Core.Components.Interfaces;
using WeBoard.Core.Updates.Interfaces;

namespace WeBoard.Core.Updates.Base
{
    public abstract class UpdateBase : IUpdate
    {
        public long TargetId { get;}
        public DateTime Date { get; }

        public UpdateBase(long id)
        {
            TargetId = id;
            Date = DateTime.UtcNow;

        }

        public abstract void Apply(ITrackable trackable);
        public abstract IUpdate GetCancelUpdate();

    }
}
