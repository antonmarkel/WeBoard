using WeBoard.Core.Components.Interfaces;

namespace WeBoard.Core.Updates.Interfaces
{
    public interface IUpdate
    {
        public long TargetId { get;}
        public DateTime Date { get;}
        public void Apply(ITrackable trackable);
        IUpdate GetCancelUpdate();
    }
}
