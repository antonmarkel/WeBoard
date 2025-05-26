using SFML.Graphics;
using SFML.System;
using WeBoard.Client.Services.Interfaces.Base;
using WeBoard.Client.Services.Managers;
using WeBoard.Core.Components.Interfaces;
using WeBoard.Core.Components.Shapes;

namespace WeBoard.Client.Services
{
    public class TestService : IService
    {
        private readonly ComponentManager componentManager = ComponentManager.GetInstance();
        private DateTime _lastUpdatedAt = DateTime.Now;
        private DateTime _lastCleanUpAt = DateTime.Now;
        private double _afterLastShift = 0;
        private const int StepMs = 1000;
        private const int CleanUpIntervalMs = 5000;

        public void OnUpdate()
        {
            _afterLastShift += (DateTime.Now - _lastUpdatedAt).TotalMilliseconds;
            _lastUpdatedAt = DateTime.Now;

            while (_afterLastShift >= StepMs)
            {
                _afterLastShift -= StepMs;
                ShiftShapes();
            }

            if ((DateTime.Now - _lastCleanUpAt).TotalMilliseconds >= CleanUpIntervalMs)
            {
                CleanUp();
                _lastCleanUpAt = DateTime.Now;
            }
        }

        private void ShiftShapes()
        {

            var count = componentManager.Count;
            var random = new Random();
            var flag = random.Next() % 3 == 0;

            if (flag && count > 3)
            {
                var index = random.Next(0, count);
                var renderList = componentManager.GetComponentsForLogic().ToList();
                var toRemoveDrawable = renderList[index];

                return;
            }

            var rectangle = new Rectangle(new Vector2f(random.Next(100, 300), random.Next(100, 300)),
                new Vector2f(random.Next(-1000, 1000), random.Next(-1000, 1000)))
            {
                FillColor = new Color(
                    (byte)random.Next(150, 255),
                    (byte)random.Next(150, 255),
                    (byte)random.Next(150, 255)
                )
            };
            componentManager.AddComponent(rectangle);

            var triangle = new Triangle(new Vector2f(random.Next(100, 300), random.Next(100, 300)), 
                new Vector2f(random.Next(-1000, 1000), random.Next(-1000, 1000)))
            {
                FillColor = new Color(
                    (byte)random.Next(150, 255),
                    (byte)random.Next(150, 255),
                    (byte)random.Next(150, 255)
                )
            };
            componentManager.AddComponent(triangle);

            var circle = new Ellipse(new Vector2f(random.Next(100, 300), random.Next(100, 300)),
                new Vector2f(random.Next(-1000, 1000), random.Next(-1000, 1000)))
            {
                FillColor = new Color(
                    (byte)random.Next(150, 255),
                    (byte)random.Next(150, 255),
                    (byte)random.Next(150, 255)
                )
            };
            componentManager.AddComponent(circle);
        }

        private void CleanUp()
        {
            var components = componentManager.GetComponentsForRender().ToList();
            foreach (var comp in components)
            {
                if (comp is ICleanable cleanable && cleanable.ShouldBeClean)
                {
                    componentManager.RemoveComponent(comp);
                    cleanable.ShouldBeClean = false;
                }
            }
        }
    }
}