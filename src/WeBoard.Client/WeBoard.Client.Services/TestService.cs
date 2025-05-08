using SFML.Graphics;
using SFML.System;
using WeBoard.Client.Services.Interfaces.Base;
using WeBoard.Client.Services.Managers;
using WeBoard.Core.Components.Shapes;

namespace WeBoard.Client.Services
{
    public class TestService : IService
    {
        private readonly ComponentManager componentManager  = ComponentManager.GetInstance();
        private DateTime _lastUpdatedAt = DateTime.Now;
        private double _afterLastShift = 0;
        private const int StepMs = 1000;

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

            var count = componentManager.RenderObjects.Count;
            var random = new Random();
            var flag = random.Next() % 3 == 0;

            if (flag && count > 3)
            {
                var index = random.Next(0, count);
                var renderList = componentManager.RenderObjects.ToList();
                var toRemoveDrawable = renderList[index];

                componentManager.RenderObjects.Remove(toRemoveDrawable.Key, out _);

                return;
            }

            var rectangle = new Rectangle(new RectangleShape(new Vector2f(random.Next(300, 600), random.Next(300, 600)))
            {
                FillColor = new Color((byte)random.Next(150, 255),
                    (byte)random.Next(150, 255),
                    (byte)random.Next(150, 255))
            }, new Vector2f(random.Next(-1000, 1000), random.Next(-1000, 1000)));
            componentManager.TryAddComponent(rectangle);

        }
    }
}