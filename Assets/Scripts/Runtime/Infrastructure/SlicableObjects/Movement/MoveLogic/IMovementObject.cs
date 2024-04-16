using UnityEngine;

namespace Runtime.Infrastructure.SlicableObjects.Movement.MoveLogic
{
    public interface IMovementObject
    {
        Vector2 Position { get; }

        void SimulateMovement();
    }
}