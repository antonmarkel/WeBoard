using System.Collections.Immutable;
using WeBoard.Core.Components.Base;
using WeBoard.Core.Components.Interfaces;
using WeBoard.Core.Edit.Properties.Base;
using WeBoard.Core.Network.Serializable.Interfaces;
using WeBoard.Core.Updates.Edit;
using Color = SFML.Graphics.Color;

namespace WeBoard.Core.Components.Shapes.Base
{
    public abstract class ShapeBase : InteractiveComponentBase, IEditable, ISavable
    {
        public virtual float OutlineThickness
        {
            get => Shape.OutlineThickness;
            set => Shape.OutlineThickness = value;
        }
        public virtual Color OutlineColor
        {
            get => Shape.OutlineColor;
            set => Shape.OutlineColor = value;
        }

        private void InitializeEditProperties()
        {
            var editColorEditName = "Fill color";
            var editOutlineThicknessEditName = "Outline thickness";
            var editOutlineColorEditName = "Outline color";
            var fillColorEditProperty = new EditProperty<Color>(
                editColorEditName,
                setter: value =>
                {
                    if (FillColor != value)
                        TrackUpdate(new EditUpdate(Id, editColorEditName, value, FillColor));
                    FillColor = value;
                },
                getter: () => FillColor);
            var outlineThicknessEditProperty = new EditProperty<int>(
                editOutlineThicknessEditName,
                setter: value =>
                {
                    if (Math.Abs(value - OutlineThickness) > 0.01f)
                        TrackUpdate(new EditUpdate(Id, editOutlineThicknessEditName, value, OutlineThickness));
                    OutlineThickness = value;
                },
                getter: () => (int)OutlineThickness);
            var outlineColor = new EditProperty<Color>(
                editOutlineColorEditName,
                setter: value =>
                {
                    if (OutlineColor != value)
                        TrackUpdate(new EditUpdate(Id, editOutlineColorEditName, value, OutlineColor));
                    OutlineColor = value;
                },
                getter: () => OutlineColor);

            _editProperties = [fillColorEditProperty, outlineThicknessEditProperty, outlineColor];
        }

        public override IBinarySerializable ToSerializable()
        {
            return new SerializableShape()
            {
                ZIndex = ZIndex,
                Id = Id,
                Position = Position,
                Size = GetSize(),
                Rotation = Rotation,
                FillColor = FillColor,
                OutlineColor = OutlineColor,
                OutlineThickness = OutlineThickness
            };
        }
        public override void FromSerializable(IBinarySerializable serializable)
        {
            if (serializable is SerializableShape serializableShape)
            {
                ZIndex = serializableShape.ZIndex;
                Id = serializableShape.Id;
                Position = serializableShape.Position;
                SetSize(serializableShape.Size);
                Rotation = serializableShape.Rotation;
                FillColor = serializableShape.FillColor;
                OutlineColor = serializableShape.OutlineColor;
                OutlineThickness = serializableShape.OutlineThickness;
            }
        }

        private IImmutableList<EditPropertyBase>? _editProperties;
        public virtual IImmutableList<EditPropertyBase> EditProperties
        {
            get
            {
                if (_editProperties is null)
                    InitializeEditProperties();
                return _editProperties!;
            }
        }
    }
}
