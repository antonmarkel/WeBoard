using SFML.Graphics;
using SFML.System;
using WeBoard.Client.Core.Engine;
using WeBoard.Client.Core.Engine.Interfaces;

namespace WeBoard.Client.Services
{
    public class TestService : IService
    {
        private readonly BoardGlobal _global = BoardGlobal.GetInstance();
        private DateTime _lastUpdatedAt = DateTime.Now;
        private double _afterLastShift = 0;
        private const int StepMs = 250;

        public void OnUpdate()
        {
            _afterLastShift += (DateTime.Now - _lastUpdatedAt).TotalMilliseconds;
            _lastUpdatedAt = DateTime.Now;

            while (_afterLastShift >= StepMs)
            {
                _afterLastShift -= StepMs;
                ShiftShapes();
            }
        }

        private void ShiftShapes()
        {
            
            var count = _global.RenderObjects.Count;
            var random = new Random();
            var flag = random.Next() % 3 == 0;

            if (flag && count > 3)
            {
                var index = random.Next(0, count);
                var renderList = _global.RenderObjects.ToList();
                var toRemoveDrawable = renderList[index];

                _global.RenderObjects.Remove(toRemoveDrawable.Key, out _);

                return;
            }

            var rectangle = new RectangleShape(new Vector2f(random.Next(300, 600), random.Next(300, 600)))
            {
                Position = new Vector2f(random.Next(-1000, 1000), random.Next(-1000, 1000)),
                FillColor = new Color((byte)random.Next(150, 255),
                    (byte)random.Next(150, 255),
                    (byte)random.Next(150, 255))
            };
            _global.RenderObjects.TryAdd(Guid.NewGuid(), rectangle);

        }
    }
}
