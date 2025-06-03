using SFML.Graphics;
using SFML.Window;
using WeBoard.Client.Services.Engine;
using WeBoard.Client.Services.Managers;
using WeBoard.Client.Services.Render;

string userId = args[0];
string boardId = args[1];

var mainWindow = new RenderWindow(new VideoMode(800,800), "WeBoard", Styles.Fullscreen);
var camera = new BoardCamera(mainWindow);
var renderManager = RenderManager.GetInstance();
renderManager.Camera = camera;
renderManager.RenderWindow = mainWindow;

var componentManager = ComponentManager.GetInstance();
var boardRender = new BoardRender(renderManager.RenderWindow);

BoardEngine engine = new BoardEngine();
engine.InitNetwork(userId, boardId, "http://3.98.122.179:5005"); 

engine.Start();
boardRender.Start();
