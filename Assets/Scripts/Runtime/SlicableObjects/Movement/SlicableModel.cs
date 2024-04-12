using UnityEngine;

namespace Runtime.SlicableObjects.Movement
{
    public class SlicableModel
    {
        public readonly Vector2 Direction;
        
        public float SpeedX;
        public float SpeedY;

        public SlicableModel(float speedX, float speedY, Vector2 direction)
        {
            SpeedX = speedX;
            SpeedY = speedY;
            Direction = direction;
        }
    }
}