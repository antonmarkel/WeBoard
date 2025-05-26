using SFML.Graphics;
using SFML.System;
using SFML.Window;
using WeBoard.Client.Services;
using WeBoard.Client.Services.Engine;
using WeBoard.Client.Services.Managers;
using WeBoard.Client.Services.Render;
using WeBoard.Core.Components.Tools;
using WeBoard.Core.Components.Menu.Visuals;

var mainWindow = new RenderWindow(VideoMode.DesktopMode, "WeBoard", Styles.Default);
var camera = new BoardCamera(mainWindow);
var renderManager = RenderManager.GetInstance();
renderManager.Camera = camera;
renderManager.RenderWindow = mainWindow;

var componentManager = ComponentManager.GetInstance();
var boardRender = new BoardRender(renderManager.RenderWindow);

BoardEngine engine = new BoardEngine();
engine.AddService(new TestService());
var marker = new MarkerTool();
ToolManager.GetInstance().ActiveTool = marker;
marker.SetBrushSize(14f);
marker.SetBrushColor(new Color(255, 0, 255, 100));

engine.Start();
boardRender.Start();
