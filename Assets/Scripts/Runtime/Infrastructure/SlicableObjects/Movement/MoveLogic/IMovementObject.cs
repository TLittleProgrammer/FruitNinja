using UnityEngine;

namespace Runtime.Infrastructure.SlicableObjects.Movement.MoveLogic
{
    public interface IMovementObject
    {
        public float PositionX { get; }
        public float PositionY { get; }
        Vector2 Position { get; }

        void SimulateMovement();
    }
}