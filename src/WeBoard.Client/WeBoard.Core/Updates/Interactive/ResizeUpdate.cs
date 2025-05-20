using SFML.System;
using WeBoard.Core.Components.Interfaces;
using WeBoard.Core.Updates.Base;
using WeBoard.Core.Updates.Interfaces;

namespace WeBoard.Core.Updates.Interactive
{
    public class ResizeUpdate : UpdateBase
    {
        private readonly Vector2f _offsetSize;

        public ResizeUpdate(long targetId, Vector2f offsetSize) : base(targetId)
        {
            _offsetSize = offsetSize;
        }

        public override void Apply(ITrackable trackable)
        {
            if (trackable is IResizable resizable)
            {
                var originalSize = resizable.GetSize();
                resizable.SetSize(originalSize + _offsetSize);
            }

        }

        public override IUpdate GetCancelUpdate()
        {
            return new ResizeUpdate(TargetId, -_offsetSize);
        }
    }
}
