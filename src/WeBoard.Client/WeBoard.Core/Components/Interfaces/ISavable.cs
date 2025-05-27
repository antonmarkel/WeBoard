using WeBoard.Core.Network.Serializable.Interfaces;

namespace WeBoard.Core.Components.Interfaces
{
    public interface ISavable
    {
        IBinarySerializable ToSerializable();
        void FromSerializable(IBinarySerializable serializable);
    }
}
