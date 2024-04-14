using Runtime.Extensions;
using UnityEngine;

namespace Runtime.Infrastructure.SlicableObjects.Movement.Animation
{
    public sealed class ScaleAnimation : IModelAnimation
    {
        private readonly Transform _mainTransform;
        private readonly Transform _shadowTransform;
        private readonly Vector2 _shadowDirection = new(1, -0.5f);

        private const float _mainScaleSpeed = 0.1f;
        private const float _shadowOffsetSpeed = 0.05f;

        public ScaleAnimation(Transform mainTransform, Transform shadowTransform)
        {
            _mainTransform = mainTransform;
            _shadowTransform = shadowTransform;

            //TODO потом исправить настройку позиции тени
            shadowTransform.localPosition = new(0.1f, -0.035f, 0f);
            _mainTransform.localScale = Vector3.one;
        }

        public float Rotation => _mainTransform.rotation.z;

        public void SimulateAnimation()
        {
            float mainScaleDelta   = _mainScaleSpeed * Time.deltaTime;
            float shadowOffsetDelta = _shadowOffsetSpeed * Time.deltaTime;

            Vector3 mainScale   = _mainTransform.localScale;
            
            _mainTransform.localScale   = new Vector3(mainScale.x + mainScaleDelta, mainScale.y + mainScaleDelta, 1f);

            Vector3 offset = _shadowDirection.normalized * shadowOffsetDelta;
            offset.z = 0f;
            
            _shadowTransform.localPosition += offset;
        }
    }
}