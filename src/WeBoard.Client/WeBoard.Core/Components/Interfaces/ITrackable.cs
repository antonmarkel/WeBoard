using WeBoard.Core.Updates.Interfaces;

namespace WeBoard.Core.Components.Interfaces
{
    public interface ITrackable
    {
        public List<IUpdate> Updates { get; set;}
    }
}
