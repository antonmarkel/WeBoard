using WeBoard.Core.Network.Serializable.Enums;
using WeBoard.Core.Updates.Creation;
using WeBoard.Core.Updates.Edit;
using WeBoard.Core.Updates.Enums;
using WeBoard.Core.Updates.Interactive;
using WeBoard.Core.Updates.Interfaces;

namespace WeBoard.Core.Network.Serializable.Tools
{
    public class UpdateSerializer
    {
        public static string Serialize(IUpdate update)
        {
            return update.Serialize();
        }

        public static IUpdate? Deserialize(string dataString)
        {
            byte[] bytes = Convert.FromBase64String(dataString);
            ReadOnlySpan<byte> data = bytes.AsSpan();

            BinaryHeaderDeserializer.GetHeaders(data, out byte typeId, out byte version);
            if (typeId != (byte)SerializableTypeIdEnum.Update)
                return null;

            BinaryHeaderDeserializer.GetHeadersForUpdate(data, out byte updateId);

            UpdateEnum updateEnum = (UpdateEnum)updateId;
            IUpdate? update = null;

            switch (updateEnum)
            {
                case UpdateEnum.CreateUpdate:
                    update = new CreateUpdate(0, string.Empty);
                    break;
                case UpdateEnum.RemoveUpdate:
                    update = new ResizeUpdate(0, new());
                    break;
                case UpdateEnum.EditUpdate:
                    update = new EditUpdate(0, string.Empty, string.Empty, string.Empty);
                    break;
                case UpdateEnum.DragUpdate:
                    update = new DragUpdate(0, new());
                    break;
                case UpdateEnum.ResizeUpdate:
                    update = new ResizeUpdate(0, new());
                    break;
                case UpdateEnum.RotateUpdate:
                    update = new RotateUpdate(0, 0);
                    break;
            }

            if (update is null)
                return update;

            update.Deserialize(dataString);
            return update;
        }
    }
}
