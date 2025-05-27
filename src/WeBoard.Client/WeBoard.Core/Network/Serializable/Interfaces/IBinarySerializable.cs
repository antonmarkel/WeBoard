namespace WeBoard.Core.Network.Serializable.Interfaces
{
    public interface IBinarySerializable
    {
        byte TypeId { get; }
        byte Version { get; }
        Type SerializableType { get; }
        ReadOnlySpan<byte> Serialize();
        void Deserialize(ReadOnlySpan<byte> data);
    }
}
