
using WeBoard.Core.Components.Shapes;
using WeBoard.Core.Network.Serializable.Enums;

namespace WeBoard.Core.Network.Serializable.Shapes
{
    public class SerializableRectangle : SerializableShape
    {
        public SerializableRectangle() : base()
        {
        }
        public SerializableRectangle(SerializableShape another) : base(another)
        {
        }
        public Type SerializableType => typeof(Rectangle);
        public override byte TypeId => (byte)SerializableTypeIdEnum.Rectangle;
        public override byte Version => 0x01;
    }
}
