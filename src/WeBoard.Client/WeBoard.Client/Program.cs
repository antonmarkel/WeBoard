using SFML.Graphics;
using SFML.Window;
using WeBoard.Client.Services;
using WeBoard.Client.Services.Engine;
using WeBoard.Client.Services.Managers;
using WeBoard.Client.Services.Render;
using WeBoard.Core.Components.Tools;

var mainWindow = new RenderWindow(VideoMode.DesktopMode, "WeBoard", Styles.Default);
var camera = new BoardCamera(mainWindow);
var renderManager = RenderManager.GetInstance();
renderManager.Camera = camera;
renderManager.RenderWindow = mainWindow;

var componentManager = ComponentManager.GetInstance();
var boardRender = new BoardRender(renderManager.RenderWindow);

BoardEngine engine = new BoardEngine();
engine.AddService(new TestService());
ToolManager.GetInstance().ActiveTool = new BrushTool();

engine.Start();
boardRender.Start();
