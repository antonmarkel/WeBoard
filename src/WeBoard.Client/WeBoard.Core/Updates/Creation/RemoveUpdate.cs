using System.Buffers.Binary;
using System.Text;
using WeBoard.Core.Components.Interfaces;
using WeBoard.Core.Network.Serializable.Interfaces;
using WeBoard.Core.Updates.Base;
using WeBoard.Core.Updates.Enums;
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

        public override UpdateEnum UpdateType => UpdateEnum.RemoveUpdate;
        public override Type SerializableType => typeof(RemoveUpdate);
        public override void Deserialize(string dataString)
        {
            byte[] bytes = Convert.FromBase64String(dataString);
            ReadOnlySpan<byte> data = bytes.AsSpan();
            int offset = 0;

            DeserializeBase(data, ref offset);

            int dataLength = BinaryPrimitives.ReadInt32LittleEndian(data.Slice(offset, 4));
            offset += 4;

            _data = Encoding.UTF8.GetString(data.Slice(offset, dataLength));
        }
        public override string Serialize()
        {
            var dataLength = Encoding.UTF8.GetByteCount(_data);
            int totalLength = UpdateBaseSize + 4 + dataLength;
            Span<byte> span = new byte[totalLength];
            int offset = 0;

            SerializeBase(span, ref offset);

            BinaryPrimitives.WriteInt32LittleEndian(span.Slice(offset, 4), dataLength);
            offset += 4;

            Encoding.UTF8.GetBytes(_data, span.Slice(offset, dataLength));
            return Convert.ToBase64String(span.ToArray());
        }


    }
}
