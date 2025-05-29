using System.Buffers.Binary;
using System.Collections.Immutable;
using SFML.Graphics;
using SFML.System;
using WeBoard.Core.Drawables.Strokes;
using WeBoard.Core.Network.Serializable.Enums;
using WeBoard.Core.Network.Serializable.Interfaces;

namespace WeBoard.Core.Network.Serializable.Strokes
{
    public class StrokeSerializable : IBinarySerializable
    {
        public byte TypeId { get; }
        public byte Version => 0x01;
        public Type SerializableType { get; }
        public int ZIndex { get; set; }
        public int Id { get; set; }
        public Vector2f Position { get; set; }
        public Vector2f Size { get; set; }
        public Color Color { get; set; }
        public float Radius { get;set; }
        public ImmutableList<Vector2f> Dots { get; set; } = [];

        public StrokeSerializable(byte typeId)
        {
            if (typeId == (byte)SerializableTypeIdEnum.Brush)
            {
                SerializableType = typeof(BrushStroke);
                TypeId = typeId;
            }
            else
            {
                SerializableType = typeof(PencilStroke);
                TypeId = (byte)SerializableTypeIdEnum.Pencil;
            }
        }

        public string Serialize()
        {
            // Calculate total length:
            // Header: 2 bytes
            // Properties: 4+4 + 4*2 + 4*2 + 4 + 4 (dots count) + 8 * dots.Length
            int totalLength = 2 + 4 + 4 + (4 * 2) + (4 * 2) + 4 + 4 + 4 + (Dots.Count * 8);

            Span<byte> span = new byte[totalLength];

            int offset = 0;

            span[offset++] = TypeId;
            span[offset++] = Version;

            BinaryPrimitives.WriteInt32LittleEndian(span.Slice(offset, 4), ZIndex);
            offset += 4;
            BinaryPrimitives.WriteInt32LittleEndian(span.Slice(offset, 4), Id);
            offset += 4;

            WriteFloat(span.Slice(offset, 4), Position.X);
            offset += 4;
            WriteFloat(span.Slice(offset, 4), Position.Y);
            offset += 4;

            // Size
            WriteFloat(span.Slice(offset, 4), Size.X);
            offset += 4;
            WriteFloat(span.Slice(offset, 4), Size.Y);
            offset += 4;

            span[offset++] = Color.R;
            span[offset++] = Color.G;
            span[offset++] = Color.B;
            span[offset++] = Color.A;

            WriteFloat(span.Slice(offset, 4), Radius);
            offset += 4;

            BinaryPrimitives.WriteInt32LittleEndian(span.Slice(offset, 4), Dots.Count);
            offset += 4;

            foreach (var dot in Dots)
            {
                WriteFloat(span.Slice(offset, 4), dot.X);
                offset += 4;
                WriteFloat(span.Slice(offset, 4), dot.Y);
                offset += 4;
            }

            return Convert.ToBase64String(span.ToArray());
        }

        public void Deserialize(string dataString)
        {
            byte[] bytes = Convert.FromBase64String(dataString);
            ReadOnlySpan<byte> data = bytes.AsSpan();
            int offset = 0;

            if (data[offset++] != TypeId || data[offset++] != Version)
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

            Color = new Color(
                data[offset++],
                data[offset++],
                data[offset++],
                data[offset++]
            );

            Radius = ReadFloat(data.Slice(offset, 4));
            offset += 4;

            int dotsCount = BinaryPrimitives.ReadInt32LittleEndian(data.Slice(offset, 4));
            offset += 4;

            var temp = new Vector2f[dotsCount];
            for (int i = 0; i < dotsCount; i++)
            {
                float x = ReadFloat(data.Slice(offset, 4));
                offset += 4;
                float y = ReadFloat(data.Slice(offset, 4));
                offset += 4;
                temp[i] = new Vector2f(x, y);
            }

            Dots = temp.ToImmutableList();
        }

        private static void WriteFloat(Span<byte> destination, float value)
        {
            BinaryPrimitives.WriteInt32LittleEndian(destination, BitConverter.SingleToInt32Bits(value));
        }

        private static float ReadFloat(ReadOnlySpan<byte> source)
        {
            return BitConverter.Int32BitsToSingle(BinaryPrimitives.ReadInt32LittleEndian(source));
        }
    }
}
