using SFML.System;
using WeBoard.Core.Components.Interfaces;
using WeBoard.Core.Updates.Base;
using WeBoard.Core.Updates.Interfaces;

namespace WeBoard.Core.Updates.Interactive
{
    public class DragUpdate : UpdateBase
    {
        private readonly Vector2f _offset;

        public DragUpdate(int targetId,Vector2f offset) : base(targetId)
        {
            _offset = offset;
        }

        public override IUpdate GetCancelUpdate()
        {
            return new DragUpdate(TargetId, -_offset);
        }

        public override void UpdateActionMethod(ITrackable trackable)
        {
            if (trackable is IDraggable draggable)
            {
                draggable.Drag(_offset);
                draggable.OnStopDragging();
            }
        }
    }
}
