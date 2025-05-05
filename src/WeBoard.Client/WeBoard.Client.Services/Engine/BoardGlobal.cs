using System.Collections.Concurrent;
using SFML.Graphics;

namespace WeBoard.Client.Core.Engine
{
    public class BoardGlobal
    {
        private static readonly BoardGlobal Instance = new();
        public ConcurrentDictionary<Guid, Drawable> RenderObjects { get; set; } = [];
        public RenderWindow? RenderWindow { get; set; }
        public View? RenderView { get; set; }
        public BoardGlobal()
        {
          
        }

        public static BoardGlobal GetInstance()
        {
            return Instance;
        }
    }
}
