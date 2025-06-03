using SFML.Graphics;
using SFML.Window;
using WeBoard.Client.Services.Engine;
using WeBoard.Client.Services.Managers;
using WeBoard.Client.Services.Render;

var mainWindow = new RenderWindow(new VideoMode(800,800), "WeBoard", Styles.Default);
var camera = new BoardCamera(mainWindow);
var renderManager = RenderManager.GetInstance();
renderManager.Camera = camera;
renderManager.RenderWindow = mainWindow;

var componentManager = ComponentManager.GetInstance();
var boardRender = new BoardRender(renderManager.RenderWindow);

BoardEngine engine = new BoardEngine();
engine.InitNetwork("91732bf6-fee0-4526-975d-0c85c7a54ce3", "61c4a00d-8744-4dd6-85dd-a280d2dae68d", "http://3.98.122.179:5005");

engine.Start();
boardRender.Start();
