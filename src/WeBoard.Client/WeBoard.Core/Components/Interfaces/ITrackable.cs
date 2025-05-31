using WeBoard.Core.Updates.Interfaces;

namespace WeBoard.Core.Components.Interfaces
{
    public interface ITrackable
    {
        public bool IsUpdating { get; set; }
        public void TrackUpdate(IUpdate update);
        public List<IUpdate> Updates { get; set; }
    }
}
