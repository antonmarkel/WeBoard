using System.Collections.Immutable;
using SFML.Graphics;
using SFML.System;
using WeBoard.Core.Components.Base;
using WeBoard.Core.Components.Interfaces;
using WeBoard.Core.Components.Menu.Containers;
using WeBoard.Core.Components.Menu.Inputs;
using WeBoard.Core.Components.Menu.Visuals;
using WeBoard.Core.Edit.Base;
using WeBoard.Core.Edit.Properties.Base;

namespace WeBoard.Core.Edit
{
    public class NumberEdit : EditBase<int>, IContainer
    {
        private HorizontalStackContainer _container;
        private LabelComponent _label;
        private TextInputComponent _input;
        public NumberEdit(EditProperty<int> property) : base(property)
        {
            _label = new LabelComponent($"{property.Name}:", new Vector2f());
            _input = new TextInputComponent(new Vector2f(), new Vector2f(_label.Size.Y*3, _label.Size.Y*1.5f),fontSize:19);
            _input.Content = property.GetValue().ToString();
            _input.OnInputKey += ch =>
            {
                if (!"0123456789".Contains(ch))
                    _input.Content = _input.Content[..^1];
            };
            _input.OnInput += str =>
            {
                int value;
                if (!int.TryParse(str, out value))
                {
                    value = 1;
                    _input.Content = value.ToString();
                }

                property.UpdateValue(value);
            };

            _container = new([_label,_input]);

            _container.Padding = new Vector2f(5, 5);
            _container.SpaceBetween = 10;
            _container.CornerRadius = 0;
            _container.CornerPointCount = 2;
        }

        public override Vector2f Position
        {
            get => _container.Position;
            set => _container.Position = value;
        }

        protected override Shape Shape => _container.FocusShape;

        public ImmutableList<MenuComponentBase> Children => _container.Children;

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
