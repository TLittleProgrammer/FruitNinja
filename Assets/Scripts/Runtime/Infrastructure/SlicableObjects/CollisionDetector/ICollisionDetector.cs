using Zenject;

namespace Runtime.Infrastructure.SlicableObjects.CollisionDetector
{
    public interface ICollisionDetector<TCollider, TType> : ILateTickable
    {
        void AddCollider(TCollider collider, TType obj);
        void RemoveCollider(TCollider collider);
    }
}