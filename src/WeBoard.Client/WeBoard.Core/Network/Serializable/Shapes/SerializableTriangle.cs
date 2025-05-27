using WeBoard.Core.Components.Shapes;
using WeBoard.Core.Network.Serializable.Enums;

namespace WeBoard.Core.Network.Serializable.Shapes
{
    public class SerializableTriangle : SerializableShape
    {
        public SerializableTriangle() : base()
        {
        }

        public SerializableTriangle(SerializableShape another) : base(another)
        {
        }

        public Type SerializableType => typeof(Triangle);
        public override byte TypeId => (byte)SerializableTypeIdEnum.Triangle;
        public override byte Version => 0x01;
    }
}
