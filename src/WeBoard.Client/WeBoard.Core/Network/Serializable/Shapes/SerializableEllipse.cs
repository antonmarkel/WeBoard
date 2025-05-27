using WeBoard.Core.Components.Shapes;
using WeBoard.Core.Network.Serializable.Enums;

namespace WeBoard.Core.Network.Serializable.Shapes
{
    public class SerializableEllipse : SerializableShape
    {
        public SerializableEllipse() : base()
        {
        }
        public SerializableEllipse(SerializableShape another) : base(another)
        {
        }
        public Type SerializableType => typeof(Ellipse);
        public override byte TypeId => (byte)SerializableTypeIdEnum.Ellipse;
        public override byte Version => 0x01;
    }
}
