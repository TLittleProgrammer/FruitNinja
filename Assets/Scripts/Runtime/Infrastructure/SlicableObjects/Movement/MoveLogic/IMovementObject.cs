using UnityEngine;

namespace Runtime.Infrastructure.SlicableObjects.Movement.MoveLogic
{
    public interface IMovementObject
    {
        public Vector2 Direction { get; set; }
        public float SpeedX { get; }
        public float SpeedY { get; }
        Vector2 Position { get; }

        void SimulateMovement();
    }
}