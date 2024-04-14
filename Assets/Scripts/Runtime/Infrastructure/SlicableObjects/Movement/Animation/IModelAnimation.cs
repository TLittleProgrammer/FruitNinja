namespace Runtime.Infrastructure.SlicableObjects.Movement.Animation
{
    public interface IModelAnimation
    {
        void SimulateAnimation();
        float Rotation { get; }
    }
}