using System.Buffers.Binary;
using WeBoard.Core.Components.Interfaces;
using WeBoard.Core.Network.Serializable.Enums;
using WeBoard.Core.Updates.Enums;
using WeBoard.Core.Updates.Interfaces;

namespace WeBoard.Core.Updates.Base
{
    public abstract class UpdateBase : IUpdate
    {
        public int TargetId { get; private set; }
        public DateTime Date { get; private set; }
        private Action<ITrackable> UpdateAction => UpdateActionMethod;

        public byte TypeId => (byte)SerializableTypeIdEnum.Update;
        public byte Version => 0x01;
        public virtual UpdateEnum UpdateType { get; private set; }
        public abstract Type SerializableType { get; }

        public UpdateBase(int id)
        {
            TargetId = id;
            Date = DateTime.UtcNow;

        }

        public abstract void UpdateActionMethod(ITrackable trackable);
        public void Apply(ITrackable trackable)
        {
            trackable.IsUpdating = true;
            UpdateAction(trackable);
            trackable.IsUpdating = false;
        }
        public abstract IUpdate GetCancelUpdate();
        public abstract string Serialize();
        public abstract void Deserialize(string data);

        protected const int UpdateBaseSize = 15;
        protected void SerializeBase(Span<byte> span, ref int offset)
        {
            span[offset++] = TypeId;
            span[offset++] = Version;

            span[offset++] = (byte)UpdateType;

            BinaryPrimitives.WriteInt32LittleEndian(span.Slice(offset, 4), TargetId);
            offset += 4;

            BinaryPrimitives.WriteInt64LittleEndian(span.Slice(offset, 8), Date.Ticks);
            offset += 8;
        }

        protected void DeserializeBase(ReadOnlySpan<byte> span, ref int offset)
        {
            if (span[offset++] != TypeId || span[offset++] != Version)
                return;

            UpdateType = (UpdateEnum)span[offset++];

            TargetId = BinaryPrimitives.ReadInt32LittleEndian(span.Slice(offset, 4));
            offset += 4;
            var dateTicks = BinaryPrimitives.ReadInt64LittleEndian(span.Slice(offset, 8));
            offset += 8;

            Date = new DateTime(dateTicks);
        }
    }
}
