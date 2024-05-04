using Runtime.Infrastructure.Timer;
using UnityEngine;

namespace Runtime.Infrastructure.SlicableObjects.Movement.Animation
{
    public sealed class ScaleAnimation : IModelAnimation
    {
        private readonly Transform _mainTransform;
        private readonly Transform _shadowTransform;
        private readonly ITimeProvider _timeProvider;
        private readonly Vector2 _shadowDirection = new(1, -0.5f);

        private const float MainScaleSpeed = 0.1f;
        private const float ShadowOffsetSpeed = 0.15f;

        public ScaleAnimation(Transform mainTransform, Transform shadowTransform, ITimeProvider timeProvider)
        {
            _mainTransform = mainTransform;
            _shadowTransform = shadowTransform;
            _timeProvider = timeProvider;

            shadowTransform.localPosition = new(0.1f, -0.035f, 0f);
            _mainTransform.localScale = Vector3.one;
        }

        public float Rotation => _mainTransform.rotation.z;

        public void SimulateAnimation()
        {
            if (_mainTransform.localScale.x >= 1.25f)
                return;
            
            float mainScaleDelta   = MainScaleSpeed * _timeProvider.DeltaTime;
            float shadowOffsetDelta = ShadowOffsetSpeed * _timeProvider.DeltaTime;

            Vector3 mainScale = _mainTransform.localScale;
            
            _mainTransform.localScale = new Vector3(mainScale.x + mainScaleDelta, mainScale.y + mainScaleDelta, 1f);

            Vector3 offset = _shadowDirection.normalized * shadowOffsetDelta;
            offset.z = 0f;
            
            _shadowTransform.localPosition += offset;
        }
    }
}