using System.Collections.Concurrent;
using SFML.Graphics;
using WeBoard.Client.Services.Render;

namespace WeBoard.Client.Core.Engine
{
    public class BoardGlobal
    {
        private static BoardGlobal? Instance;
        public ConcurrentDictionary<Guid, Drawable> RenderObjects { get; set; } = [];
        public RenderWindow RenderWindow { get; set; }
        public BoardCamera Camera { get; set; }
        public BoardGlobal(RenderWindow window,BoardCamera camera)
        {
            RenderWindow = window;
            Camera = camera;

            Instance = this;
        }

        public static BoardGlobal GetInstance()
        {
            return Instance!;
        }
    }
}
