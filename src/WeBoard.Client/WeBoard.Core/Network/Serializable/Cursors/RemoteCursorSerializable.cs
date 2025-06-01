using System.Buffers.Binary;
using SFML.System;
using WeBoard.Core.Drawables.Cursors;
using WeBoard.Core.Network.Serializable.Enums;
using WeBoard.Core.Network.Serializable.Interfaces;

namespace WeBoard.Core.Network.Serializable.Cursors
{
    public class RemoteCursorSerializable : IBinarySerializable
    {
        public byte TypeId => (byte)SerializableTypeIdEnum.RemoteCursor;
        public byte Version => 0x01;
        public Type SerializableType => typeof(RemoteCursor);

        public long UserId { get; set; }
        public Vector2f Position { get; set; }

        public void Deserialize(string dataString)
        {
            byte[] bytes = Convert.FromBase64String(dataString);
            ReadOnlySpan<byte> data = bytes.AsSpan();
            int offset = 0;

            if (data[offset++] != TypeId || data[offset++] != Version)
                return;

            UserId = BinaryPrimitives.ReadInt64LittleEndian(data.Slice(offset, 8));
            offset += 8;

            var posX = IBinarySerializable.ReadFloat(data.Slice(offset, 4));
            offset += 4;
            var posY = IBinarySerializable.ReadFloat(data.Slice(offset, 4));

            Position = new Vector2f(posX, posY);
        }

        public string Serialize()
        {
            int totalLength = 1 + 1 + 8 + 4 + 4;
            Span<byte> span = new byte[totalLength];
            int offset = 0;

            span[offset++] = TypeId;
            span[offset++] = Version;

            BinaryPrimitives.WriteInt64LittleEndian(span.Slice(offset, 8), UserId);
            offset += 8;

            IBinarySerializable.WriteFloat(span.Slice(offset, 4), Position.X);
            offset += 4;
            IBinarySerializable.WriteFloat(span.Slice(offset, 4), Position.Y);

            return Convert.ToBase64String(span.ToArray());
        }
    }
}
