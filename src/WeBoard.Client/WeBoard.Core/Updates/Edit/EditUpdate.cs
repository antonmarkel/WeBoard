using System.Text.Json;
using SFML.Graphics;
using WeBoard.Core.Components.Interfaces;
using WeBoard.Core.Updates.Base;
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
    }
}
