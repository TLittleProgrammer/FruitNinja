using Runtime.Constants;
using UnityEngine;

namespace Runtime.Infrastructure.SlicableObjects.Movement.MoveLogic
{
    public sealed class MovementObjectService : IMovementObject
    {
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
            _position = _startPosition;
            _movementTransform = movementTransform;
            _velocityX = velocityX;
            _velocityY = velocityY;
            _angle = angle;
            _allTime = 0f;
        }

        public float PositionX => _velocityX * _allTime * Mathf.Cos(_angle);
        public float PositionY => _velocityY * _allTime * Mathf.Sin(_angle) - 0.5f * (-World.Gravity * Mathf.Pow(_allTime, 2));
        
        public Vector2 Position => _position;

        public void SimulateMovement()
        {
            _allTime += Time.deltaTime;
            
            _position = new Vector2(PositionX, PositionY);
            _movementTransform.position = _position + _startPosition;
        }
    }
}