using SFML.System;
using System.Buffers.Binary;
using System.Text;
using WeBoard.Core.Components.Visuals;
using WeBoard.Core.Network.Serializable.Enums;
using WeBoard.Core.Network.Serializable.Interfaces;

namespace WeBoard.Core.Network.Serializable.Visuals
{
    public class SerializableText : IBinarySerializable
    {
        public int ZIndex { get; set; }
        public int Id { get; set; }
        public Vector2f Position { get; set; }
        public Vector2f Size { get; set; }
        public float Rotation { get; set; }
        public string Text { get; set; } = string.Empty;
        public Type SerializableType => typeof(TextComponent);
        public virtual byte TypeId => (byte)SerializableTypeIdEnum.Text;
        public virtual byte Version => 0x01;

        public string Serialize()
        {
            // Calculate byte size:
            // 1 byte TypeID + 1 byte Version
            // 4 + 4 (ints)
            // 4 + 4 (Position floats)
            // 4 + 4 (Size floats)
            // 4 (Rotation)
            // 4 (OutlineThickness)
            // 4 (OutlineColor bytes)
            // 4 (FillColor bytes)
            int totalLength = 1 + 1 + 4 + 4 + (4 * 2) + (4 * 2) + 4 + 4 + Text.Length;

            Span<byte> span = stackalloc byte[totalLength];

            int offset = 0;

            span[offset++] = TypeId; // Type ID for 
            span[offset++] = Version;    // Version

            BinaryPrimitives.WriteInt32LittleEndian(span.Slice(offset, 4), ZIndex);
            offset += 4;

            BinaryPrimitives.WriteInt32LittleEndian(span.Slice(offset, 4), Id);
            offset += 4;

            WriteFloat(span.Slice(offset, 4), Position.X);
            offset += 4;
            WriteFloat(span.Slice(offset, 4), Position.Y);
            offset += 4;

            WriteFloat(span.Slice(offset, 4), Size.X);
            offset += 4;
            WriteFloat(span.Slice(offset, 4), Size.Y);
            offset += 4;

            WriteFloat(span.Slice(offset, 4), Rotation);
            offset += 4;


            BinaryPrimitives.WriteInt32LittleEndian(span.Slice(offset, 4), Text.Length);
            offset += 4;

            Encoding.UTF8.GetBytes(Text, span.Slice(offset, Text.Length));

            return Convert.ToBase64String(span.ToArray());
        }

        public void Deserialize(string dataString)
        {
            byte[] bytes = Convert.FromBase64String(dataString);
            ReadOnlySpan<byte> data = bytes.AsSpan();

            int offset = 0;

            byte typeId = data[offset++];
            byte version = data[offset++];

            if (typeId != TypeId)
                return;

            if (version != Version)
                return;

            ZIndex = BinaryPrimitives.ReadInt32LittleEndian(data.Slice(offset, 4));
            offset += 4;

            Id = BinaryPrimitives.ReadInt32LittleEndian(data.Slice(offset, 4));
            offset += 4;

            float posX = ReadFloat(data.Slice(offset, 4));
            offset += 4;
            float posY = ReadFloat(data.Slice(offset, 4));
            offset += 4;
            Position = new Vector2f(posX, posY);

            float sizeX = ReadFloat(data.Slice(offset, 4));
            offset += 4;
            float sizeY = ReadFloat(data.Slice(offset, 4));
            offset += 4;
            Size = new Vector2f(sizeX, sizeY);

            Rotation = ReadFloat(data.Slice(offset, 4));
            offset += 4;

            int textLength = BinaryPrimitives.ReadInt32LittleEndian(data.Slice(offset, 4));
            offset += 4;
            Text = Encoding.UTF8.GetString(data.Slice(offset, textLength));
        }

        private static void WriteFloat(Span<byte> destination, float value)
        {
            BinaryPrimitives.WriteInt32LittleEndian(destination, BitConverter.SingleToInt32Bits(value));
        }

        private static float ReadFloat(ReadOnlySpan<byte> source)
        {
            int intVal = BinaryPrimitives.ReadInt32LittleEndian(source);
            return BitConverter.Int32BitsToSingle(intVal);
        }
    }
}
