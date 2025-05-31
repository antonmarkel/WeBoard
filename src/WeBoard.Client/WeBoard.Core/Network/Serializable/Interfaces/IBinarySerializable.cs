using System.Buffers.Binary;

namespace WeBoard.Core.Network.Serializable.Interfaces
{
    public interface IBinarySerializable
    {
        byte TypeId { get; }
        byte Version { get; }
        Type SerializableType { get; }
        string Serialize();
        void Deserialize(string data);

        public static void WriteFloat(Span<byte> destination, float value)
        {
            BinaryPrimitives.WriteInt32LittleEndian(destination, BitConverter.SingleToInt32Bits(value));
        }
        public static float ReadFloat(ReadOnlySpan<byte> source)
        {
            return BitConverter.Int32BitsToSingle(BinaryPrimitives.ReadInt32LittleEndian(source));
        }
    }
}
