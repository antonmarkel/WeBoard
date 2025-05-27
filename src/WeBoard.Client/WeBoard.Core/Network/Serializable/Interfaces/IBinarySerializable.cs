namespace WeBoard.Core.Network.Serializable.Interfaces
{
    public interface IBinarySerializable
    {
        byte TypeId { get; }
        byte Version { get; }
        Type SerializableType { get; }
        string Serialize();
        void Deserialize(string data);
    }
}
