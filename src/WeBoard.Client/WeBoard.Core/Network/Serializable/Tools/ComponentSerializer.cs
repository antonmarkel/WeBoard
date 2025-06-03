using SFML.Graphics;
using WeBoard.Core.Components.Base;
using WeBoard.Core.Components.Interfaces;
using WeBoard.Core.Components.Shapes;
using WeBoard.Core.Components.Visuals;
using WeBoard.Core.Drawables.Strokes;
using WeBoard.Core.Network.Serializable.Enums;
using WeBoard.Core.Network.Serializable.Interfaces;
using WeBoard.Core.Network.Serializable.Shapes;
using WeBoard.Core.Network.Serializable.Strokes;
using WeBoard.Core.Network.Serializable.Visuals;

namespace WeBoard.Core.Network.Serializable.Tools
{
    public class ComponentSerializer
    {
        public static string Serialize(ISavable savable)
        {
            var serializable = savable.ToSerializable();
            return serializable.Serialize();
        }

        public static InteractiveComponentBase? Deserialize(string dataString)
        {
            byte[] bytes = Convert.FromBase64String(dataString);
            ReadOnlySpan<byte> data = bytes.AsSpan();

            BinaryHeaderDeserializer.GetHeaders(data, out byte typeId, out byte version);

            SerializableTypeIdEnum typeEnum = (SerializableTypeIdEnum)typeId;

            IBinarySerializable? binarySerializable;
            InteractiveComponentBase? component = null;

            switch (typeEnum)
            {
                case SerializableTypeIdEnum.Rectangle:
                    binarySerializable = new SerializableRectangle();
                    component = new Rectangle(new(), new());
                    break;
                case SerializableTypeIdEnum.Ellipse:
                    binarySerializable = new SerializableEllipse();
                    component = new Ellipse(new(), new());
                    break;
                case SerializableTypeIdEnum.Triangle:
                    binarySerializable = new SerializableTriangle();
                    component = new Triangle(new(), new());
                    break;
                case SerializableTypeIdEnum.Brush:
                    binarySerializable = new StrokeSerializable((byte)SerializableTypeIdEnum.Brush);
                    component = new BrushStroke(Color.Black, 0);
                    break;
                case SerializableTypeIdEnum.Pencil:
                    binarySerializable = new StrokeSerializable((byte)SerializableTypeIdEnum.Pencil);
                    component = new PencilStroke(Color.Black);
                    break;
                case SerializableTypeIdEnum.Text:
                    binarySerializable = new SerializableText();
                    component = new TextComponent(new());
                    break;
                default:
                    binarySerializable = null;
                    break;
            }

            if (binarySerializable is null)
                return null;

            binarySerializable.Deserialize(dataString);
            component!.FromSerializable(binarySerializable);

            return component;

        }
    }
}
