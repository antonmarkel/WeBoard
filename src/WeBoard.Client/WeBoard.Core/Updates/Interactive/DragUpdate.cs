using SFML.System;
using WeBoard.Core.Components.Interfaces;
using WeBoard.Core.Updates.Base;
using WeBoard.Core.Updates.Interfaces;

namespace WeBoard.Core.Updates.Interactive
{
    public class DragUpdate : UpdateBase
    {
        private readonly Vector2f _offset;

        public DragUpdate(long targetId,Vector2f offset) : base(targetId)
        {
            _offset = offset;
        }

        public override void Apply(ITrackable trackable)
        {
            if (trackable is IDraggable draggable)
            {
                draggable.Drag(_offset);
            }
                
        }

        public override IUpdate GetCancelUpdate()
        {
            return new DragUpdate(TargetId, -_offset);
        }
    }
}
