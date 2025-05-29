using System.Buffers.Binary;
using SFML.Graphics;
using SFML.System;
using WeBoard.Core.Network.Serializable.Interfaces;

public class SerializableShape : IBinarySerializable
{
    public SerializableShape(SerializableShape another)
    {
        ZIndex = another.ZIndex;
        Id = another.Id;
        Position = another.Position;
        Size = another.Size;
        Rotation = another.Rotation;
        OutlineThickness = another.OutlineThickness;
        OutlineColor = another.OutlineColor;
        FillColor = another.FillColor;
    }

    public SerializableShape()
    {
    }

    public int ZIndex { get; set; }
    public int Id { get; set; }
    public Vector2f Position { get; set; }
    public Vector2f Size { get; set; }
    public float Rotation { get; set; }
    public float OutlineThickness { get; set; }
    public Color OutlineColor { get; set; }
    public Color FillColor { get; set; }
    public Type SerializableType { get; }

    public virtual byte TypeId { get; }
    public virtual byte Version { get; }

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
        int totalLength = 1 + 1 + 4 + 4 + (4 * 2) + (4 * 2) + 4 + 4 + 4 + 4;

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

        WriteFloat(span.Slice(offset, 4), OutlineThickness);
        offset += 4;

        // OutlineColor (RGBA bytes)
        span[offset++] = OutlineColor.R;
        span[offset++] = OutlineColor.G;
        span[offset++] = OutlineColor.B;
        span[offset++] = OutlineColor.A;

        // FillColor (RGBA bytes)
        span[offset++] = FillColor.R;
        span[offset++] = FillColor.G;
        span[offset++] = FillColor.B;
        span[offset] = FillColor.A;

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

        OutlineThickness = ReadFloat(data.Slice(offset, 4));
        offset += 4;

        OutlineColor = new Color(
            data[offset++],
            data[offset++],
            data[offset++],
            data[offset++]
        );

        FillColor = new Color(
            data[offset++],
            data[offset++],
            data[offset++],
            data[offset]
        );
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
