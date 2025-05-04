using SFML.Graphics;
using SFML.System;
using SFML.Window;
using WeBoard.Client.Board;
using WeBoard.Client.Core.Engine;
using WeBoard.Client.Services;

var mainWindow = new RenderWindow(VideoMode.DesktopMode, "WeBoard", Styles.Default);
var boardRender = new BoardRender(mainWindow);
var globals = BoardGlobal.GetInstance();
globals.RenderWindow = mainWindow;
globals.RenderObjects.TryAdd(Guid.NewGuid(), new RectangleShape(new Vector2f(100, 100))
{
    FillColor = Color.Blue,
    Position = new Vector2f(200, 200)
});

BoardEngine engine = new BoardEngine();
engine.Start();
engine.AddService(new TestService());

boardRender.Start();
