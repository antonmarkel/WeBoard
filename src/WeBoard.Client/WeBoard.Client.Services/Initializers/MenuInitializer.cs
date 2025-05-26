using System.Collections.Immutable;
using SFML.Graphics;
using SFML.System;
using WeBoard.Client.Services.Managers;
using WeBoard.Core.Components.Base;
using WeBoard.Core.Components.Content;
using WeBoard.Core.Components.Menu.Buttons;
using WeBoard.Core.Components.Menu.Buttons.RadioButtons;
using WeBoard.Core.Components.Menu.Containers;
using WeBoard.Core.Components.Menu.Inputs;
using WeBoard.Core.Components.Menu.Visuals;
using WeBoard.Core.Components.Shapes;
using WeBoard.Core.Edit;
using WeBoard.Core.Edit.Properties.Base;
using WeBoard.Core.Enums.Menu;

namespace WeBoard.Client.Services.Initializers
{
    public class MenuInitializer
    {
        private readonly MenuManager _menuManager;

        public MenuInitializer()
        {
            _menuManager = MenuManager.GetInstance();
        }

        private static RadioButtonComponent CopyButton(ButtonComponent button)
        {
            return new RadioButtonComponent(button.Position, button.Size)
            {
                BackgroundColor = button.BackgroundColor,
                ContentView = button.ContentView,
                CornerRadius = button.CornerRadius,
                CornerPointCount = button.CornerPointCount,
                OutlineThickness = button.OutlineThickness,
                Padding = button.Padding,
            };
        }

        private void InitializeShapeSideMenu(RadioButtonComponent shapeRadioButton, List<MenuComponentBase> menuComponents)
        {
            var button = new ButtonComponent(new(), new());
            button.BackgroundColor = Color.Red;
            var testColor = new ColorEdit(new EditProperty<Color>("Color", val => button.BackgroundColor = val,
                () => button.BackgroundColor))
            {
                Position = new(500, 500)
            };

            var testInput = new TextInputComponent(new Vector2f(700,700), new Vector2f(50 * 3, 50));
            testInput.Content = "1";
            var rectContent = new InteractiveComponentContent(
                new Rectangle(new Vector2f(40, 40), new Vector2f(0, 0))
                {
                    FillColor = Color.Red,
                    OutlineColor = Color.Black,
                    OutlineThickness = 1
                });
            var triangleContent = new InteractiveComponentContent(
                new Triangle(new Vector2f(40, 40), new Vector2f(0, 0))
                {
                    FillColor = Color.Blue,
                    OutlineColor = Color.Black,
                    OutlineThickness = 1
                });
            var circleContent = new InteractiveComponentContent(
                new Ellipse(new Vector2f(40, 40), new Vector2f(0, 0))
                {
                    FillColor = Color.Green,
                    OutlineColor = Color.Black,
                    OutlineThickness = 1
                });

            var rectButton = new RadioButtonComponent(new Vector2f(0, 0), new Vector2f(30, 30))
            {
                BackgroundColor = new Color(255, 255, 255, 255),
                ContentView = rectContent,
                CornerRadius = 5,
                CornerPointCount = 20,
                OutlineThickness = 1,
                Padding = 7
            };
            var triangleButton = CopyButton(rectButton);
            triangleButton.ContentView = triangleContent;
            var circleButton = CopyButton(rectButton);
            circleButton.ContentView = circleContent; ;

            rectButton.OnGroupUpdate += self =>
            {
                if (self)
                {
                    _menuManager.CurrentInstrument = InstrumentOptionsEnum.ShapeRectangle;
                    shapeRadioButton.ContentView = rectContent;
                }
            };
            triangleButton.OnGroupUpdate += self =>
            {
                if (self)
                {
                    _menuManager.CurrentInstrument = InstrumentOptionsEnum.ShapeTriangle;
                    shapeRadioButton.ContentView = triangleContent;
                }
            };
            circleButton.OnGroupUpdate += self =>
            {
                if (self)
                {
                    _menuManager.CurrentInstrument = InstrumentOptionsEnum.ShapeCircle;
                    shapeRadioButton.ContentView = circleContent;
                }
            };

            var shapeRadioGroup = new RadioButtonGroup();
            rectButton.Group = triangleButton.Group = circleButton.Group = shapeRadioGroup;

            shapeRadioGroup.SetOption(rectButton);
            var shapeStack = new HorizontalStackContainer([rectButton, triangleButton, circleButton])
            {
                BackgroundColor = new Color(255, 255, 255, 255),
                OutlineThickness = 1f,
                OutlineColor = Color.Black,
                Padding = new Vector2f(5, 5),
                SpaceBetween = 10f,
                Position = new Vector2f(40, 200),
                CornerRadius = 0,
                CornerPointCount = 40,
                IsHidden = true
            };

            shapeRadioButton.OnGroupUpdate += self =>
            {
                if (self)
                {
                    shapeStack.Show();
                    shapeStack.Position = new Vector2f(shapeRadioButton.Position.X + 70, shapeRadioButton.Position.Y);
                }
                else
                    shapeStack.Hide();
            };

            menuComponents.AddRange([testColor, testInput,shapeStack]);
        }

        private void InitializeSideMenu(List<MenuComponentBase> menuComponents)
        {
            var pencilContent = new ImageContentView(new Texture("Resources/Menu/pencil.png"));
            var brushContent = new ImageContentView(new Texture("Resources/Menu/brush.png"));
            var eraserContent = new ImageContentView(new Texture("Resources/Menu/eraser.png"));
            var cursorContent = new ImageContentView(new Texture("Resources/Menu/cursor.png"));
            var fontContent = new ImageContentView(new Texture("Resources/Menu/font.png"));

            var pencilButton = new RadioButtonComponent(new Vector2f(0, 0), new Vector2f(50, 50))
            {
                BackgroundColor = new Color(255, 255, 255, 255),
                ContentView = pencilContent,
                CornerRadius = 5,
                CornerPointCount = 20,
                OutlineThickness = 1
            };
            var brushButton = CopyButton(pencilButton);
            brushButton.ContentView = brushContent;
            var eraserButton = CopyButton(pencilButton);
            eraserButton.ContentView = eraserContent;
            var cursorButton = CopyButton(pencilButton);
            cursorButton.ContentView = cursorContent;
            var shapeButton = CopyButton(pencilButton);
            shapeButton.Padding = 7;
            var textButton = CopyButton(pencilButton);
            textButton.ContentView = fontContent;

            pencilButton.OnGroupUpdate += self =>
            {
                if (self)
                    _menuManager.CurrentInstrument = InstrumentOptionsEnum.Pencil;
            };
            brushButton.OnGroupUpdate += self =>
            {
                if (self)
                    _menuManager.CurrentInstrument = InstrumentOptionsEnum.Brush;
            };
            eraserButton.OnGroupUpdate += self =>
            {
                if (self)
                    _menuManager.CurrentInstrument = InstrumentOptionsEnum.Eraser;
            };
            cursorButton.OnGroupUpdate += self =>
            {
                if (self)
                    _menuManager.CurrentInstrument = InstrumentOptionsEnum.Cursor;
            };
            textButton.OnGroupUpdate += self =>
            {
                if (self)
                    _menuManager.CurrentInstrument = InstrumentOptionsEnum.Text;
            };



            var radioGroup = new RadioButtonGroup();
            pencilButton.Group = brushButton.Group = cursorButton.Group = shapeButton.Group = textButton.Group = eraserButton.Group = radioGroup;

            List<MenuComponentBase> sideButtons = [cursorButton, pencilButton, brushButton, eraserButton, textButton, shapeButton];
            var verticalStack = new VerticalStackContainer(sideButtons)
            {
                BackgroundColor = new Color(255, 255, 255, 80),
                OutlineThickness = 1f,
                OutlineColor = Color.Black,
                Padding = new Vector2f(15, 7),
                SpaceBetween = 15f,
                Position = new Vector2f(-10, 200),
                CornerRadius = 10,
                CornerPointCount = 40,
            };
            menuComponents.Add(verticalStack);
            InitializeShapeSideMenu(shapeButton,menuComponents);
        }

        public IImmutableList<MenuComponentBase> InitializeComponents()
        {
            var menuComponents = new List<MenuComponentBase>();
            InitializeSideMenu(menuComponents);

            return menuComponents.ToImmutableList();
        }
    }
}
