using System.Buffers.Binary;
using System.Text;
using System.Text.Json;
using SFML.Graphics;
using WeBoard.Core.Components.Interfaces;
using WeBoard.Core.Updates.Base;
using WeBoard.Core.Updates.Enums;
using WeBoard.Core.Updates.Interfaces;

namespace WeBoard.Core.Updates.Edit
{
    public class EditUpdate : UpdateBase
    {
        private string _jsonValue;
        private string _jsonLastValue;
        private string _editPropertyName;

        private EditUpdate(int targetId, string editPropertyName, string jsonValue, string jsonLastValue) : base(targetId)
        {
            _editPropertyName = editPropertyName;
            _jsonLastValue = jsonLastValue;
            _jsonValue = jsonValue;
        }

        public EditUpdate(int targetId, string editPropertyName, object newValue, object lastValue) : base(targetId)
        {
            _editPropertyName = editPropertyName;
            if (newValue is Color newColor)
                newValue = newColor.ToInteger();
            if (lastValue is Color lastColor)
                lastValue = lastColor.ToInteger();

            _jsonValue = JsonSerializer.Serialize(newValue);
            _jsonLastValue = JsonSerializer.Serialize(lastValue);
        }

        public override IUpdate GetCancelUpdate()
        {
            return new EditUpdate(TargetId, _editPropertyName, _jsonLastValue, _jsonValue);
        }

        public override void UpdateActionMethod(ITrackable trackable)
        {
            if (trackable is IEditable editable)
            {
                var editProperty = editable.EditProperties.FirstOrDefault(prop => prop.Name == _editPropertyName);
                if (editProperty is null)
                    return;

                if (editProperty.ValueType == typeof(Color))
                {
                    var colorValue = JsonSerializer.Deserialize<uint>(_jsonValue);
                    var color = new Color(colorValue);

                    editProperty.UpdateValue(color);
                    return;
                }

                var value = JsonSerializer.Deserialize(_jsonValue, editProperty.ValueType);
                if (value is null)
                    return;

                editProperty.UpdateValue(value);
            }
        }

        public override UpdateEnum UpdateType => UpdateEnum.EditUpdate;
        public override Type SerializableType => typeof(EditUpdate);
        public override void Deserialize(string dataString)
        {
            byte[] bytes = Convert.FromBase64String(dataString);
            ReadOnlySpan<byte> data = bytes.AsSpan();
            int offset = 0;

            DeserializeBase(data, ref offset);

            int jsonValueLength = BinaryPrimitives.ReadInt32LittleEndian(data.Slice(offset, 4));
            offset += 4;
            _jsonValue = Encoding.UTF8.GetString(data.Slice(offset, jsonValueLength));
            offset += jsonValueLength;

            int jsonLastValueLength = BinaryPrimitives.ReadInt32LittleEndian(data.Slice(offset, 4));
            offset += 4;
            _jsonLastValue = Encoding.UTF8.GetString(data.Slice(offset, jsonLastValueLength));
            offset += jsonLastValueLength;

            int editPropertyLength = BinaryPrimitives.ReadInt32LittleEndian(data.Slice(offset, 4));
            offset += 4;
            _editPropertyName = Encoding.UTF8.GetString(data.Slice(offset, editPropertyLength));

        }
        public override string Serialize()
        {
            var jsonValueLength = Encoding.UTF8.GetByteCount(_jsonValue);
            var jsonLastValueLength = Encoding.UTF8.GetByteCount(_jsonLastValue);
            var editPropertyLength = Encoding.UTF8.GetByteCount(_editPropertyName);

            int totalLength = UpdateBaseSize + 4 * 3 + jsonValueLength + jsonLastValueLength + editPropertyLength;
            Span<byte> span = new byte[totalLength];
            int offset = 0;

            SerializeBase(span, ref offset);

            BinaryPrimitives.WriteInt32LittleEndian(span.Slice(offset, 4), jsonValueLength);
            offset += 4;
            Encoding.UTF8.GetBytes(_jsonValue, span.Slice(offset, jsonValueLength));
            offset += jsonValueLength;

            BinaryPrimitives.WriteInt32LittleEndian(span.Slice(offset, 4), jsonLastValueLength);
            offset += 4;
            Encoding.UTF8.GetBytes(_jsonLastValue, span.Slice(offset, jsonLastValueLength));
            offset += jsonLastValueLength;

            BinaryPrimitives.WriteInt32LittleEndian(span.Slice(offset, 4), editPropertyLength);
            offset += 4;
            Encoding.UTF8.GetBytes(_editPropertyName, span.Slice(offset, editPropertyLength));

            return Convert.ToBase64String(span.ToArray());
        }

    }
}
