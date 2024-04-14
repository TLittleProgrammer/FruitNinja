using UnityEngine;

namespace Runtime.Infrastructure.SlicableObjects.HideCondition
{
    public sealed class SliceableConditionHideObjectService : IConditionObjectHideService
    {
        private readonly Camera _camera;
        private readonly GameScreenManager _gameScreenManager;
        private readonly Transform _transform;
        private bool _metOnScreen;

        public SliceableConditionHideObjectService(GameScreenManager gameScreenManager, Transform transform)
        {
            _gameScreenManager = gameScreenManager;
            _transform = transform;
            _metOnScreen = false;
        }
        
        public bool IsNeedHideObject()
        {
            bool objectAtScreen = _gameScreenManager.WorldPositionAtScreenRect(_transform.position);

            if (objectAtScreen && _metOnScreen is false)
            {
                _metOnScreen = true;
                return false;
            }

            if (objectAtScreen is false && _metOnScreen)
            {
                return true;
            }

            return false;
        }

        public void Reset()
        {
            _metOnScreen = false;
        }
    }
}