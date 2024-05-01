using Runtime.Constants;
using UnityEngine;

namespace Runtime.Infrastructure.SlicableObjects.Movement.MoveLogic
{
    public sealed class MovementObjectService : IMovementObjectService
    {
        private float _offsetX;
        private float _offsetY;
        private float _accelerationOfGravity;
        
        private Transform _movementTransform;
        private Vector2 _position;
        private float _velocityX;
        private float _velocityY;
        private float _angle;
        private float _constantValueInSpeedYFormula;
        private float _allTime;
        private Vector2 _startPosition;

        public MovementObjectService(Transform movementTransform, float velocityX, float velocityY, float angle)
        {
            _startPosition = movementTransform.position;
            _movementTransform = movementTransform;
            
            Initialize(velocityX, velocityY, angle);
        }

        private void Initialize(float velocityX, float velocityY, float angle)
        {
            _velocityX = velocityX;
            _velocityY = velocityY;
            _angle = angle;
            _allTime = 0f;

            _offsetX = _velocityX * Mathf.Cos(_angle);
            _offsetY = _velocityY * Mathf.Sin(_angle);
            _accelerationOfGravity = World.Gravity;
        }

        public float PositionY => _offsetY * _allTime + _accelerationOfGravity * Mathf.Pow(_allTime, 2);
        public Vector2 Position => _position + _startPosition;

        public void SimulateMovement()
        {
            _allTime += Time.deltaTime;
            _offsetY += _accelerationOfGravity * Time.deltaTime;
            
            _position = new Vector2(_position.x + _offsetX * Time.deltaTime, _position.y + Time.deltaTime * _offsetY);
            _movementTransform.position = Position;
        }

        public void Reset(float velocityY, float velocityX, float angle, Vector3 startPosition)
        {
            _allTime = 0f;
            _startPosition = startPosition;
            Initialize(velocityX, velocityY, angle);
        }

        public void AddSpeed(Vector2 speed)
        {
            _offsetX += speed.x;
            _offsetY += speed.y;
        }
    }
}