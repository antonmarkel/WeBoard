using SFML.Graphics;
using SFML.System;
using SFML.Window;
using WeBoard.Client.Services.Render;
using WeBoard.Core.Components.Interfaces;

namespace WeBoard.Client.Services.Managers
{
    public class RenderManager
    {
        private static RenderManager? Instance;
        public RenderWindow RenderWindow { get; set; }
        public BoardCamera Camera { get; set; }

        public RenderManager()
        {
            Instance = this;
        }

        public static RenderManager GetInstance()
        {
            return Instance ?? new RenderManager();
        }
    }
}
