using WeBoard.Core.Components.Interfaces;
using WeBoard.Core.Network.Serializable.Interfaces;
using WeBoard.Core.Updates.Base;
using WeBoard.Core.Updates.Interfaces;

namespace WeBoard.Core.Updates.Creation
{
    public class RemoveUpdate : UpdateBase
    {
        protected string _data;

        public RemoveUpdate(int id, IBinarySerializable serializable) : base(id)
        {
            _data = serializable.Serialize();
        }
        public RemoveUpdate(int id, string data) : base(id)
        {
            _data = data;
        }

        public override IUpdate GetCancelUpdate()
        {
            return new CreateUpdate(TargetId, _data);
        }

        public override void UpdateActionMethod(ITrackable trackable)
        {

        }
    }
}
