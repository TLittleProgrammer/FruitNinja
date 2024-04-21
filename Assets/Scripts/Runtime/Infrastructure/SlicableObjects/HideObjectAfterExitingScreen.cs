using Runtime.Infrastructure.Game;
using Runtime.Infrastructure.SlicableObjects.HideCondition;
using Runtime.Infrastructure.SlicableObjects.Movement;
using UnityEngine;
using Zenject;

namespace Runtime.Infrastructure.SlicableObjects
{
    public class HideObjectAfterExitingScreen : MonoBehaviour
    {
        [SerializeField] private bool _isSlicableObject = true;
        [SerializeField] private SlicableObjectView _slicableObjectView;
        
        private SlicableMovementService _slicableMovementService;
        private GameParameters _gameParameters;
        private IConditionObjectHideService _conditionObjectHideService;
        private GameScreenManager _gameScreenManager;

        [Inject]
        private void Construct(
            SlicableMovementService slicableMovementService,
            GameScreenManager gameScreenManager,
            GameParameters gameParameters)
        {
            _gameScreenManager = gameScreenManager;
            _gameParameters = gameParameters;
            _slicableMovementService = slicableMovementService;
        }

        private void LateUpdate()
        {
            //TODO Подумать над тем, чтобы перенести
            if (_conditionObjectHideService is null)
            {
                _conditionObjectHideService = new SimpleConditionHideObjectService(transform, _gameScreenManager.GetHorizontalSizeWithStep(), -_gameScreenManager.GetOrthographicSize() - 1.5f);
            }
            
            if (_conditionObjectHideService.IsNeedHideObject())
            {
                gameObject.SetActive(false);
                _slicableMovementService.RemoveFromMapping(transform);

                if (_isSlicableObject && _slicableObjectView.SlicableObjectType is SlicableObjectType.Simple)
                {
                    _gameParameters.ChangeHealth(-1);
                }
            }
        }
    }
}