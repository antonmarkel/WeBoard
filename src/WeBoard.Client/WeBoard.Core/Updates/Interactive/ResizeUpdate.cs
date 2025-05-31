using SFML.System;
using WeBoard.Core.Components.Interfaces;
using WeBoard.Core.Network.Serializable.Interfaces;
using WeBoard.Core.Updates.Base;
using WeBoard.Core.Updates.Enums;
using WeBoard.Core.Updates.Interfaces;

namespace WeBoard.Core.Updates.Interactive
{
    public class ResizeUpdate : UpdateBase
    {
        private Vector2f _offsetSize;

        public ResizeUpdate(int targetId, Vector2f offsetSize) : base(targetId)
        {
            _offsetSize = offsetSize;
        }

        public override void UpdateActionMethod(ITrackable trackable)
        {
            if (trackable is IResizable resizable)
            {
                var originalSize = resizable.GetSize();
                resizable.SetSize(originalSize + _offsetSize);
            }

        }
        public override IUpdate GetCancelUpdate()
        {
            return new ResizeUpdate(TargetId, -_offsetSize);
        }
        public override UpdateEnum UpdateType => UpdateEnum.ResizeUpdate;
        public override Type SerializableType => typeof(ResizeUpdate);
        public override void Deserialize(string dataString)
        {
            byte[] bytes = Convert.FromBase64String(dataString);
            ReadOnlySpan<byte> data = bytes.AsSpan();
            int offset = 0;

            DeserializeBase(data, ref offset);

            var offsetX = IBinarySerializable.ReadFloat(data.Slice(offset, 4));
            offset += 4;
            var offsetY = IBinarySerializable.ReadFloat(data.Slice(offset, 4));

            _offsetSize = new Vector2f(offsetX, offsetY);
        }
        public override string Serialize()
        {
            int totalLength = UpdateBaseSize + 4 + 4;
            Span<byte> span = new byte[totalLength];
            int offset = 0;

            SerializeBase(span, ref offset);

            IBinarySerializable.WriteFloat(span.Slice(offset, 4), _offsetSize.X);
            offset += 4;
            IBinarySerializable.WriteFloat(span.Slice(offset, 4), _offsetSize.Y);

            return Convert.ToBase64String(span.ToArray());
        }
    }
}
