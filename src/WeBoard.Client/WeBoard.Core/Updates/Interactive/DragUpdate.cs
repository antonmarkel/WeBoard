using SFML.System;
using WeBoard.Core.Components.Interfaces;
using WeBoard.Core.Network.Serializable.Interfaces;
using WeBoard.Core.Updates.Base;
using WeBoard.Core.Updates.Enums;
using WeBoard.Core.Updates.Interfaces;

namespace WeBoard.Core.Updates.Interactive
{
    public class DragUpdate : UpdateBase
    {
        private Vector2f _offset;

        public DragUpdate(int targetId, Vector2f offset) : base(targetId)
        {
            _offset = offset;
        }

        public override IUpdate GetCancelUpdate()
        {
            return new DragUpdate(TargetId, -_offset);
        }

        public override void UpdateActionMethod(ITrackable trackable)
        {
            if (trackable is IDraggable draggable)
            {
                draggable.Drag(_offset);
                draggable.OnStopDragging();
            }
        }

        public override UpdateEnum UpdateType => UpdateEnum.DragUpdate;
        public override Type SerializableType => typeof(DragUpdate);
        public override void Deserialize(string dataString)
        {
            byte[] bytes = Convert.FromBase64String(dataString);
            ReadOnlySpan<byte> data = bytes.AsSpan();
            int offset = 0;

            DeserializeBase(data, ref offset);

            var offsetX = IBinarySerializable.ReadFloat(data.Slice(offset, 4));
            offset += 4;
            var offsetY = IBinarySerializable.ReadFloat(data.Slice(offset, 4));

            _offset = new Vector2f(offsetX, offsetY);
        }
        public override string Serialize()
        {
            int totalLength = UpdateBaseSize + 4 + 4;
            Span<byte> span = new byte[totalLength];
            int offset = 0;

            SerializeBase(span, ref offset);

            IBinarySerializable.WriteFloat(span.Slice(offset, 4), _offset.X);
            offset += 4;
            IBinarySerializable.WriteFloat(span.Slice(offset, 4), _offset.Y);

            return Convert.ToBase64String(span.ToArray());
        }
    }
}
