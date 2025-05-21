using System.Collections.Immutable;
using SFML.Graphics;
using SFML.System;
using WeBoard.Core.Components.Base;
using WeBoard.Core.Components.Content;
using WeBoard.Core.Components.Menu.Buttons;
using WeBoard.Core.Components.Menu.Buttons.RadioButtons;
using WeBoard.Core.Components.Menu.Containers;
using WeBoard.Core.Components.Shapes;

namespace WeBoard.Client.Services.Initializers
{
    public class MenuInitializer
    {
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

        private VerticalStackContainer InitializeShapeSideMenu(RadioButtonComponent shapeRadioButton, List<MenuComponentBase> menuComponents)
        {
            var rectContent = new InteractiveComponentContent(
                new Rectangle(new Vector2f(40, 40), new Vector2f(0, 0))
                {
                    FillColor = Color.Red,
                });
            var triangleContent = new InteractiveComponentContent(
                new Triangle(new Vector2f(40, 40), new Vector2f(0, 0))
                {
                    FillColor = Color.Blue,
                });
            var circleContent = new InteractiveComponentContent(
                new Ellipse(new Vector2f(40, 40), new Vector2f(0, 0))
                {
                    FillColor = Color.Green,
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
                    shapeRadioButton.ContentView = rectContent;
            };
            triangleButton.OnGroupUpdate += self =>
            {
                if (self)
                    shapeRadioButton.ContentView = triangleContent;
            };
            circleButton.OnGroupUpdate += self =>
            {
                if (self)
                    shapeRadioButton.ContentView = circleContent;
            };

            var shapeRadioGroup = new RadioButtonGroup();
            rectButton.Group = triangleButton.Group = circleButton.Group = shapeRadioGroup;

            shapeRadioGroup.SetOption(rectButton);
            var shapeStack = new VerticalStackContainer([rectButton, triangleButton, circleButton])
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

            menuComponents.AddRange([rectButton,triangleButton,circleButton,shapeStack]);

            return shapeStack;
        }

        private void InitializeSideMenu(List<MenuComponentBase> menuComponents)
        {
            var pencilContent = new ImageContentView(new Texture("Resources/Menu/pencil.png"));
            var brushContent = new ImageContentView(new Texture("Resources/Menu/brush.png"));
            var cursorContent = new ImageContentView(new Texture("Resources/Menu/cursor.png"));

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
            var cursorButton = CopyButton(pencilButton);
            cursorButton.ContentView = cursorContent;
            var shapeButton = CopyButton(pencilButton);
            shapeButton.Padding = 7;

            var radioGroup = new RadioButtonGroup();
            pencilButton.Group = brushButton.Group = cursorButton.Group = shapeButton.Group = radioGroup;

            var verticalStack = new VerticalStackContainer([pencilButton,brushButton,cursorButton,shapeButton])
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
            menuComponents.AddRange([pencilButton, brushButton, cursorButton, shapeButton,verticalStack]);
            var shapeStack = InitializeShapeSideMenu(shapeButton,menuComponents);
        }

        public IImmutableList<MenuComponentBase> InitializeComponents()
        {
            var menuComponents = new List<MenuComponentBase>();
            InitializeSideMenu(menuComponents);

            return menuComponents.ToImmutableList();
        }
    }
}
