using UnityEngine;

namespace Runtime.Infrastructure.SlicableObjects.Movement.MoveLogic
{
    public interface IMovementObjectService
    {
        Vector2 Position { get; }

        void SimulateMovement();
        void Reset(float velocityX, float velocityY, float angle, Vector3 startPosition);
    }
}