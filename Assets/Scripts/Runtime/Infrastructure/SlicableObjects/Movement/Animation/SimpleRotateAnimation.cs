using Runtime.Extensions;
using Runtime.Infrastructure.Timer;
using UnityEngine;

namespace Runtime.Infrastructure.SlicableObjects.Movement.Animation
{
    public sealed class SimpleRotateAnimation : IModelAnimation
    {
        private readonly ITimeProvider _timeProvider;
        private Transform _movementTransform;
        private float _currentAngle;
        private float _rotateSpeed;

        public SimpleRotateAnimation(Transform movementTransform, float currentAngle, ITimeProvider timeProvider)
        {
            _timeProvider = timeProvider;
            InitializeVariables(movementTransform, currentAngle);
        }

        private void InitializeVariables(Transform movementTransform, float currentAngle)
        {
            _movementTransform = movementTransform;

            //TODO потом перенести
            _currentAngle = currentAngle;
            _rotateSpeed = Random.Range(50f, 150f) * GetRotateDirection();
        }

        public float Rotation => _currentAngle;
        
        public void SimulateAnimation()
        {
            if (_currentAngle.Abs() >= 360f)
            {
                _currentAngle = 0f;
            }

            _currentAngle += _rotateSpeed * _timeProvider.DeltaTime;
            _movementTransform.rotation = Quaternion.Euler(0f, 0f, _currentAngle);
        }
        
        private int GetRotateDirection()
        {
            return Random.Range(-1, 1) == -1 ? -1 : 1;
        }
    }
}