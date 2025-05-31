namespace WeBoard.Core.Updates.Enums
{
    public enum UpdateEnum : byte
    {
        CreateUpdate = 0,
        RemoveUpdate,

        EditUpdate,

        DragUpdate,
        ResizeUpdate,
        RotateUpdate,
    }
}
