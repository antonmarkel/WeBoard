using SFML.Graphics;
using SFML.System;
using SFML.Window;
using WeBoard.Client.Core.Engine;
using WeBoard.Client.Services;
using WeBoard.Client.Services.Engine;
using WeBoard.Client.Services.Render;
using WeBoard.Core.Components.Shapes;

var mainWindow = new RenderWindow(VideoMode.DesktopMode, "WeBoard", Styles.Default);
var boardRender = new BoardRender(mainWindow);
var globals = BoardGlobal.GetInstance();
globals.RenderWindow = mainWindow;
globals.RenderObjects.TryAdd(Guid.NewGuid(),new Rectangle( new RectangleShape(new Vector2f(100, 100))
{
    FillColor = Color.Blue,
    Position = new Vector2f(200, 200)
}));

BoardEngine engine = new BoardEngine();
engine.AddService(new TestService());
engine.Start();
boardRender.Start();
