using WeBoard.Core.Components.Interfaces;
using WeBoard.Core.Network.Serializable.Interfaces;
using WeBoard.Core.Updates.Base;
using WeBoard.Core.Updates.Enums;
using WeBoard.Core.Updates.Interfaces;

namespace WeBoard.Core.Updates.Interactive
{
    public class RotateUpdate : UpdateBase
    {
        private float _offsetRotate;

        public RotateUpdate(int targetId, float offsetRotate) : base(targetId)
        {
            _offsetRotate = offsetRotate;
        }

        public override void UpdateActionMethod(ITrackable trackable)
        {
            if (trackable is IRotatable rotatable)
            {
                var originalRotation = rotatable.Rotation;
                rotatable.SetRotation(originalRotation + _offsetRotate);
            }

        }

        public override IUpdate GetCancelUpdate()
        {
            return new RotateUpdate(TargetId, -_offsetRotate);
        }

        public override UpdateEnum UpdateType => UpdateEnum.RotateUpdate;
        public override Type SerializableType => typeof(RotateUpdate);
        public override void Deserialize(string dataString)
        {
            byte[] bytes = Convert.FromBase64String(dataString);
            ReadOnlySpan<byte> data = bytes.AsSpan();
            int offset = 0;

            DeserializeBase(data, ref offset);

            _offsetRotate = IBinarySerializable.ReadFloat(data.Slice(offset, 4));

        }
        public override string Serialize()
        {
            int totalLength = UpdateBaseSize + 4;
            Span<byte> span = new byte[totalLength];
            int offset = 0;

            SerializeBase(span, ref offset);
            IBinarySerializable.WriteFloat(span.Slice(offset, 4), _offsetRotate);

            return Convert.ToBase64String(span.ToArray());
        }
    }
}
