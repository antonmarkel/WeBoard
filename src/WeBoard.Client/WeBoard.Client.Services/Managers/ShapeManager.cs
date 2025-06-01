using SFML.Graphics;
using SFML.System;
using WeBoard.Core.Components.Shapes;
using WeBoard.Core.Components.Shapes.Base;
using WeBoard.Core.Enums.Menu;

namespace WeBoard.Client.Services.Managers
{
    public class ShapeManager
    {
        private static readonly ShapeManager _instance = new();
        public static ShapeManager GetInstance() => _instance;

        public ShapeBase? CreateShape(InstrumentOptionsEnum type, Vector2f position)
        {
            return type switch
            {
                InstrumentOptionsEnum.ShapeRectangle => new Rectangle(new Vector2f(100, 100), position)
                {
                    FillColor = Color.Red
                },
                InstrumentOptionsEnum.ShapeCircle => new Ellipse(new Vector2f(100, 100), position)
                {
                    FillColor = Color.Green
                },
                InstrumentOptionsEnum.ShapeTriangle => new Triangle(new Vector2f(100, 100), position)
                {
                    FillColor = Color.Blue
                },
                _ => null
            };
        }
    }
}