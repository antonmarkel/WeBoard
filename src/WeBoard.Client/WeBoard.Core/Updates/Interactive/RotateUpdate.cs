using WeBoard.Core.Components.Interfaces;
using WeBoard.Core.Updates.Base;
using WeBoard.Core.Updates.Interfaces;

namespace WeBoard.Core.Updates.Interactive
{
    public class RotateUpdate : UpdateBase
    {
        private readonly float _offsetRotate;

        public RotateUpdate(long targetId, float offsetRotate) : base(targetId)
        {
            _offsetRotate = offsetRotate;
        }

        public override void UpdateActionMethod(ITrackable trackable)
        {
            if (trackable is IRotatable rotatable)
            {
                var originalRotation = rotatable.Rotation;
                rotatable.SetRotation(originalRotation + _offsetRotate);
            }

        }

        public override IUpdate GetCancelUpdate()
        {
            return new RotateUpdate(TargetId, -_offsetRotate);
        }
    }
}
