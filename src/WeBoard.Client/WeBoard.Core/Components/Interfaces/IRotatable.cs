namespace WeBoard.Core.Components.Interfaces
{
    public interface IRotatable
    {
        float Rotation { get; set; }
        void SetRotation(float angle); 
        void OnStartRotating() { } 
        void OnStopRotating() { }
    }
}
