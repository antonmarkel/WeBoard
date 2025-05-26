using System.Collections.Immutable;
using System.Data;
using SFML.Graphics;
using WeBoard.Core.Components.Base;
using WeBoard.Core.Components.Interfaces;
using WeBoard.Core.Edit.Properties.Base;

namespace WeBoard.Core.Components.Shapes.Base
{
    public abstract class ShapeBase : InteractiveComponentBase, IEditable
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
            var fillColorEditProperty = new EditProperty<Color>(
                "Fill color",
                setter: value => FillColor = value,
                getter: () => FillColor);
            var outlineThicknessEditProperty = new EditProperty<int>(
                "Outline thickness",
                setter: value => OutlineThickness = value,
                getter: () => (int)OutlineThickness);
            var outlineColor = new EditProperty<Color>(
                "Outline color",
                setter: value => OutlineColor = value,
                getter: () => OutlineColor);

            _editProperties = [fillColorEditProperty,outlineThicknessEditProperty, outlineColor];
        }

        private IImmutableList<EditPropertyBase>? _editProperties;
        public virtual IImmutableList<EditPropertyBase> EditProperties
        {
            get
            {
                if(_editProperties is null)
                    InitializeEditProperties();
                return _editProperties!;
            }
        }
    }
}
