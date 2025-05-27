using WeBoard.Core.Components.Shapes;
using WeBoard.Core.Network.Serializable.Enums;

namespace WeBoard.Core.Network.Serializable.Shapes
{
    public class SerializableEllipse : SerializableShape
    {
        public SerializableEllipse(SerializableShape another) : base(another)
        {
        }
        public Type SerializableType => typeof(Ellipse);
        public byte TypeId => (byte)SerializableTypeIdEnum.Ellipse;
        public byte Version => 0x01;
    }
}
