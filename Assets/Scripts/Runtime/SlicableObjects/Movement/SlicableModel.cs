using UnityEngine;

namespace Runtime.SlicableObjects.Movement
{
    public class SlicableModel
    {
        private readonly Vector2 _direction;
        private readonly float _gravity;

        private float _speedX;
        private float _speedY;

        public SlicableModel(float speedX, float speedY, Vector2 direction, Vector3 position)
        {
            _speedX = speedX;
            _speedY = speedY;
            _direction = direction;
            Position = position;
            
            _gravity = Physics.gravity.y / 2f;
        }

        public Vector2 Position { get; private set; }

        public void SimulateMoving()
        {
            float deltaTime = Time.deltaTime;
            
            Position += new Vector2(_speedX, _speedY) * deltaTime * _direction;

            _speedY += _gravity * deltaTime;
        }
    }
}