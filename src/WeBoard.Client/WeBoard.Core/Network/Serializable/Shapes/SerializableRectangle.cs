
using WeBoard.Core.Components.Shapes;
using WeBoard.Core.Network.Serializable.Enums;

namespace WeBoard.Core.Network.Serializable.Shapes
{
    public class SerializableRectangle : SerializableShape
    {
        public SerializableRectangle(SerializableShape another) : base(another)
        {
        }
        public Type SerializableType => typeof(Rectangle);
        public byte TypeId => (byte)SerializableTypeIdEnum.Rectangle;
        public byte Version => 0x01;
    }
}
