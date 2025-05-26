using System.Collections.Immutable;
using SFML.Graphics;
using SFML.System;
using WeBoard.Core.Components.Base;
using WeBoard.Core.Components.Interfaces;
using WeBoard.Core.Components.Menu.Buttons;
using WeBoard.Core.Components.Menu.Containers;
using WeBoard.Core.Edit.Base;
using WeBoard.Core.Edit.Properties.Base;

namespace WeBoard.Core.Edit
{
    public class ColorEdit : Edit<Color>, IContainer
    {
        private NumberEdit _Redit;
        private NumberEdit _Gedit;
        private NumberEdit _Bedit;
        private NumberEdit _Aedit;
        private ButtonComponent _colorButton;
        private VerticalStackContainer _container;
        public ColorEdit(EditProperty<Color> property) : base(property)
        {
            _colorButton = new ButtonComponent(new(), new(50, 50));

            _colorButton.OutlineThickness = 1;
            _colorButton.OutlineColor = Color.Black;
            _colorButton.BackgroundColor = property.GetValue();
            SetColorEdits(property);

            _container = new VerticalStackContainer([_Redit!, _Gedit!, _Bedit!, _Aedit!, _colorButton]);
            _container.Padding = new Vector2f(5, 5);
            _container.SpaceBetween = 10;

        }
        public override Vector2f Position
        {
            get => _container.Position;
            set => _container.Position = value;
        }
        private void SetColorEdits(EditProperty<Color> property)
        {
            _Redit = new NumberEdit(new EditProperty<int>($"{property.Name}.R",
                setter: value =>
                {
                    var val = property.GetValue();
                    var newColor = new Color((byte)value, val.G, val.B, val.A);
                    property.UpdateValue(new Color(newColor));
                    _colorButton.BackgroundColor = newColor;
                },
                getter: () => property.GetValue().R));

            _Gedit = new NumberEdit(new EditProperty<int>($"{property.Name}.G",
                setter: value =>
                {
                    var val = property.GetValue();
                    var newColor = new Color(val.R, (byte)value, val.B, val.A);
                    property.UpdateValue(new Color(newColor));
                    _colorButton.BackgroundColor = newColor;
                },
                getter: () => property.GetValue().G));
            _Bedit = new NumberEdit(new EditProperty<int>($"{property.Name}.B",
                setter: value =>
                {
                    var val = property.GetValue();
                    var newColor = new Color(val.R, val.G, (byte)value, val.A);
                    property.UpdateValue(new Color(newColor));
                    _colorButton.BackgroundColor = newColor;
                },
                getter: () => property.GetValue().B));
            _Aedit = new NumberEdit(new EditProperty<int>($"{property.Name}.A",
                setter: value =>
                {
                    var val = property.GetValue();
                    var newColor = new Color(val.R, val.G, val.B, (byte)value);
                    property.UpdateValue(new Color(newColor));
                    _colorButton.BackgroundColor = newColor;
                },
                getter: () => property.GetValue().A));
        }

        public ImmutableList<MenuComponentBase> Children => _container.Children;

        protected override Shape Shape => _container.FocusShape;

        public override FloatRect GetScreenBounds()
        {
            return _container.GetScreenBounds();
        }

        public override void OnClick(Vector2f offset)
        {
            _container.OnClick(offset);
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            _container.Draw(target, states);
        }
    }
}
