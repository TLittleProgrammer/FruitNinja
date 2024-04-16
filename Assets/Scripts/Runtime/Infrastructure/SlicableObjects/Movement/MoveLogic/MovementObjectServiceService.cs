using Runtime.Constants;
using UnityEngine;

namespace Runtime.Infrastructure.SlicableObjects.Movement.MoveLogic
{
    public sealed class MovementObjectServiceService : IMovementObjectService
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
        
        public MovementObjectServiceService(Transform movementTransform, float velocityX, float velocityY, float angle)
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
            _accelerationOfGravity = 0.5f * World.Gravity;
        }

        public float PositionX => _offsetX * _allTime;
        public float PositionY => _offsetY * _allTime + _accelerationOfGravity * Mathf.Pow(_allTime, 2);
        public Vector2 Position => _position + _startPosition;

        public void SimulateMovement()
        {
            _allTime += Time.deltaTime;
            
            _position = new Vector2(PositionX, PositionY);
            _movementTransform.position = Position;
        }

        public void Reset(float velocityY, float velocityX, float angle, Vector3 startPosition)
        {
            _allTime = 0f;
            _startPosition = startPosition;
            Initialize(velocityX, velocityY, angle);
        }
    }
}