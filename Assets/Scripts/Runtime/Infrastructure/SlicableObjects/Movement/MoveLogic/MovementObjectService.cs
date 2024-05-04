using Runtime.Constants;
using Runtime.Infrastructure.Timer;
using UnityEngine;

namespace Runtime.Infrastructure.SlicableObjects.Movement.MoveLogic
{
    public sealed class MovementObjectService : IMovementObjectService
    {
        private readonly ITimeProvider _timeProvider;
        
        private float _offsetX;
        private float _offsetY;

        private Transform _movementTransform;
        private Vector2 _position;
        private float _velocityX;
        private float _velocityY;
        private float _angle;
        private float _constantValueInSpeedYFormula;
        private Vector2 _startPosition;

        public MovementObjectService(Transform movementTransform, float velocityX, float velocityY, float angle, ITimeProvider timeProvider)
        {
            _startPosition = movementTransform.position;
            _movementTransform = movementTransform;
            _timeProvider = timeProvider;

            Initialize(velocityX, velocityY, angle);
        }

        private void Initialize(float velocityX, float velocityY, float angle)
        {
            _velocityX = velocityX;
            _velocityY = velocityY;
            _angle = angle;

            _offsetX = _velocityX * Mathf.Cos(_angle);
            _offsetY = _velocityY * Mathf.Sin(_angle);
            _position = Vector2.zero;
        }

        public Vector2 Position => _position + _startPosition;

        public void SimulateMovement()
        {
            _offsetY += World.Gravity * _timeProvider.DeltaTime;

            _position += new Vector2(_offsetX, _offsetY) * _timeProvider.DeltaTime;
            _movementTransform.position = Position;
        }

        public void Reset(float velocityY, float velocityX, float angle, Vector3 startPosition)
        {
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