using WeBoard.Core.Components.Shapes;
using WeBoard.Core.Network.Serializable.Enums;

namespace WeBoard.Core.Network.Serializable.Shapes
{
    public class SerializableTriangle : SerializableShape
    {
        public SerializableTriangle(SerializableShape another) : base(another)
        {
        }

        public Type SerializableType => typeof(Triangle);
        public byte TypeId => (byte)SerializableTypeIdEnum.Triangle;
        public byte Version => 0x01;
    }
}
