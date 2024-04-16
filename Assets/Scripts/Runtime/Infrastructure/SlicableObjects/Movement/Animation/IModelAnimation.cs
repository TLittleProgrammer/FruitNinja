namespace Runtime.Infrastructure.SlicableObjects.Movement.Animation
{
    public interface IModelAnimation
    {
        float Rotation { get; }
        void SimulateAnimation();
    }
}