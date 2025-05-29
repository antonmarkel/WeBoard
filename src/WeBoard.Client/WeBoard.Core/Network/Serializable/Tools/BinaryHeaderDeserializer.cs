namespace WeBoard.Core.Network.Serializable.Tools
{
    public class BinaryHeaderDeserializer
    {
        public static void GetHeaders(ReadOnlySpan<byte> data, out byte typeId, out byte version)
        {
            int offset = 0;
            typeId = data[offset++];
            version = data[offset];
        }
    }
}
