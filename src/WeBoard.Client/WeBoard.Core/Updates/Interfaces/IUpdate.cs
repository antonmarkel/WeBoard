using WeBoard.Core.Components.Interfaces;
using WeBoard.Core.Network.Serializable.Interfaces;

namespace WeBoard.Core.Updates.Interfaces
{
    public interface IUpdate : IBinarySerializable
    {
        public int TargetId { get; }
        public DateTime Date { get; }
        public void Apply(ITrackable trackable);
        IUpdate GetCancelUpdate();
    }
}
