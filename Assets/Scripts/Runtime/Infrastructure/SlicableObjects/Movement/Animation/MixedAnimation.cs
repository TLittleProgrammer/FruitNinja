using UnityEngine;

namespace Runtime.Infrastructure.SlicableObjects.Movement.Animation
{
    public sealed class MixedAnimation : IModelAnimation
    {
        private SimpleRotateAnimation _simpleRotateAnimation;
        private ScaleAnimation _scaleAnimation;
        
        public MixedAnimation(Transform movementTransform, Transform shadowTransform, float currentAngle)
        {
            _simpleRotateAnimation = new(movementTransform, currentAngle);
            _scaleAnimation = new(movementTransform, shadowTransform);
        }

        public float Rotation => _simpleRotateAnimation.Rotation;

        public void SimulateAnimation()
        {
            _simpleRotateAnimation.SimulateAnimation();
            _scaleAnimation.SimulateAnimation();
        }
    }
}