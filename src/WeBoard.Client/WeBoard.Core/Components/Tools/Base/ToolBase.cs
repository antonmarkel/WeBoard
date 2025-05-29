using System.Collections.Immutable;
using SFML.Graphics;
using SFML.System;
using WeBoard.Core.Components.Base;
using WeBoard.Core.Components.Interfaces;
using WeBoard.Core.Edit.Properties.Base;

namespace WeBoard.Core.Components.Tools.Base
{
    public abstract class ToolBase : IEditable
    {
        public virtual ComponentBase? CreatedComponent { get; protected set; }
        public float ToolSize { get; set; } = 1f;
        public Color ToolColor { get; set; } = Color.Black;

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

        private void InitializeEditProperties()
        {
            var editSizeEditName = "Tool size";
            var editColorEditName = "Tool color";

            var sizeEditProperty = new EditProperty<int>(
                editSizeEditName,
                setter: value =>
                {
                    ToolSize = MathF.Max(1f, value);
                },
                getter: () => (int)ToolSize);

            var colorEditProperty = new EditProperty<Color>(
                editColorEditName,
                setter: value =>
                {
                    ToolColor = value;
                },
                getter: () => ToolColor);

            _editProperties = ImmutableList<EditPropertyBase>.Empty
                .Add(sizeEditProperty)
                .Add(colorEditProperty);
        }

        public abstract void OnMousePressed(Vector2f worldPos);
        public abstract void OnMouseReleased(Vector2f worldPos);
        public abstract void OnMouseMoved(Vector2f worldPos);
        public virtual void Draw(RenderTarget target, RenderStates states) { }
    }
}