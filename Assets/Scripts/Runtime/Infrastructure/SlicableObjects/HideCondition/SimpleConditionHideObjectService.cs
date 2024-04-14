using UnityEngine;

namespace Runtime.Infrastructure.SlicableObjects.HideCondition
{
    public sealed class SimpleConditionHideObjectService : IConditionObjectHideService
    {
        private readonly Transform _transform;
        private readonly float _horizontalMaxPosition;
        private readonly float _verticalMinPosition;

        public SimpleConditionHideObjectService(Transform transform, float horizontalMaxPosition, float verticalMinPosition)
        {
            _transform = transform;
            _horizontalMaxPosition = horizontalMaxPosition;
            _verticalMinPosition = verticalMinPosition;
        }
        
        public bool IsNeedHideObject()
        {
            Vector2 currentPosition = _transform.position;
            
            return Abs(currentPosition.x) >= _horizontalMaxPosition || currentPosition.y <= _verticalMinPosition;
        }

        public void Reset()
        {
            
        }

        private float Abs(float value)
        {
            return value > 0f ? value : -value;
        }
    }
}