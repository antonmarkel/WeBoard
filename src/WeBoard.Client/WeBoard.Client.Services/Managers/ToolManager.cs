using SFML.System;
using SFML.Graphics;
using WeBoard.Core.Components.Tools.Base;
using WeBoard.Core.Components.Tools;
using WeBoard.Core.Enums.Menu;

namespace WeBoard.Client.Services.Managers
{
    public class ToolManager
    {
        private static readonly ToolManager Instance = new();
        public static ToolManager GetInstance() => Instance;

        public ToolBase? ActiveTool { get; set; }
        private InstrumentOptionsEnum _lastTool;

        public void UpdateToolFromMenu()
        {
            var current = MenuManager.GetInstance().CurrentInstrument;
            if (current == _lastTool) return;

            _lastTool = current;

            ToolBase? tool = current switch
            {
                InstrumentOptionsEnum.Pencil => new PencilTool
                {
                    ToolColor = Color.Black,
                    ToolSize = 20f
                },
                InstrumentOptionsEnum.Brush => new BrushTool
                {
                    ToolColor = new Color(255, 0, 0, 80),
                    ToolSize = 20f
                },
                _ => null
            };

            GetInstance().ActiveTool = tool;
        }


        public void OnMousePressed(Vector2f pos)
        {
            ActiveTool?.OnMousePressed(pos);
        }

        public void OnMouseReleased(Vector2f pos)
        {
            ActiveTool?.OnMouseReleased(pos);
        }

        public void OnMouseMoved(Vector2f pos)
        {
            ActiveTool?.OnMouseMoved(pos);
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            ActiveTool?.Draw(target, states);
        }
    }
}
