using Runtime.Constants;
using Runtime.Extensions;
using Runtime.Infrastructure.SlicableObjects.Spawner;
using UnityEngine;

namespace Runtime.Infrastructure.SlicableObjects.Movement.MoveLogic
{
    public sealed class MovementObjectService : IMovementObject
    {
        private Transform _movementTransform;
        private Vector2 _position;
        private float _speedX;
        private float _speedY;
        private Vector2 _direction;
        private float _constantValueInSpeedYFormula;
        private float _allTime;
        private float _directionMultiplier = 1f;

        public MovementObjectService(Transform movementTransform, float speedX, float speedY, Vector2 direction, SideType sideType = SideType.None)
        {
            Initialize(movementTransform, speedX, speedY, direction, sideType);
        }

        private void Initialize(Transform movementTransform, float speedX, float speedY, Vector2 direction, SideType sideType)
        {
            _movementTransform = movementTransform;
            _speedX = speedX;
            _speedY = speedY;
            _direction = direction.normalized;
            _position = movementTransform.position;
            
            InitializeVariables(direction, sideType);
        }

        private void InitializeVariables(Vector2 direction, SideType sideType)
        {
            float directionAngle = direction.GetAngleBetweenVectorAndHorizontalAxis();

            directionAngle += sideType.GetAngle();
            
            _constantValueInSpeedYFormula = _speedY * Mathf.Sin(directionAngle);
            
            if (direction.y < 0f)
            {
                _directionMultiplier = -1f;
            }
        }

        public Vector2 Direction => _direction;
        public float SpeedX => _speedX;
        public float SpeedY => _speedY;
        public Vector2 Position => _position;

        public void SimulateMovement()
        {
            float deltaTime = Time.deltaTime;
            _allTime += deltaTime;

            _speedY = _constantValueInSpeedYFormula + World.Gravity * _allTime;
            
            _position += new Vector2(_speedX, _speedY * _directionMultiplier) * deltaTime * _direction;
            _movementTransform.position = _position;
        }

        private float Abs(float value)
        {
            return value > 0f ? value : -value;
        }
    }
}