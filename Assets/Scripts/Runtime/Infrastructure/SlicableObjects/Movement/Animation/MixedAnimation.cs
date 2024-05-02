using Runtime.Infrastructure.Timer;
using UnityEngine;

namespace Runtime.Infrastructure.SlicableObjects.Movement.Animation
{
    public sealed class MixedAnimation : IModelAnimation
    {
        private SimpleRotateAnimation _simpleRotateAnimation;
        private ScaleAnimation _scaleAnimation;
        
        public MixedAnimation(Transform movementTransform, Transform shadowTransform, float currentAngle, ITimeProvider timeProvider)
        {
            _simpleRotateAnimation = new(movementTransform, currentAngle, timeProvider);
            _scaleAnimation = new(movementTransform, shadowTransform, timeProvider);
        }

        public float Rotation => _simpleRotateAnimation.Rotation;

        public void SimulateAnimation()
        {
            _simpleRotateAnimation.SimulateAnimation();
            _scaleAnimation.SimulateAnimation();
        }
    }
}