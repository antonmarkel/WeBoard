using System.Text;
using WeBoard.Core.Components.Base;
using WeBoard.Core.Components.Interfaces;
using WeBoard.Core.Network.Serializable.Interfaces;
using WeBoard.Core.Network.Serializable.Tools;
using WeBoard.Core.Updates.Base;
using WeBoard.Core.Updates.Interfaces;

namespace WeBoard.Core.Updates.Creation
{
    public class CreateUpdate : UpdateBase
    {
        protected string _data;

        public CreateUpdate(int id, IBinarySerializable serializable) : base(id)
        {
            _data = serializable.Serialize();
        }
        public CreateUpdate(int id, string data) : base(id)
        {
            _data = data;
        }

        public override IUpdate GetCancelUpdate()
        {
            return new RemoveUpdate(TargetId, _data);
        }

        public InteractiveComponentBase GetComponent()
        {
            return ComponentSerializer.Deserialize(_data);
        }

        public override void UpdateActionMethod(ITrackable trackable)
        {
            
        }
    }
}
