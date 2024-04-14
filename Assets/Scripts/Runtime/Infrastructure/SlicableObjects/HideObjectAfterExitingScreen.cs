using Runtime.Infrastructure.Game;
using Runtime.Infrastructure.SlicableObjects.Movement;
using UnityEngine;
using Zenject;

namespace Runtime.Infrastructure.SlicableObjects
{
    public class HideObjectAfterExitingScreen : MonoBehaviour
    {
        [SerializeField] private bool _isSlicableObject = true;
        
        private SlicableMovementService _slicableMovementService;
        private GameScreenManager _gameScreenManager;
        private bool _metOnScreen = false;
        private GameParameters _gameParameters;

        [Inject]
        private void Construct(
            SlicableMovementService slicableMovementService,
            GameScreenManager gameScreenManager,
            GameParameters gameParameters)
        {
            _gameParameters = gameParameters;
            _gameScreenManager = gameScreenManager;
            _slicableMovementService = slicableMovementService;
        }

        private void LateUpdate()
        {
            bool objectAtScreen = _gameScreenManager.WorldPositionAtScreenRect(transform.position);

            if (objectAtScreen && _metOnScreen is false)
            {
                _metOnScreen = true;
            }

            if ((objectAtScreen is false && _metOnScreen)
                || _gameScreenManager.GetOrthographicSize() * -1 - 2 >= transform.position.y
                || _gameScreenManager.GetHorizontalSizeWithStep() + 0.15 < Mathf.Abs(transform.position.x))
            {
                gameObject.SetActive(false);
                _slicableMovementService.RemoveFromMapping(transform);
                _metOnScreen = false;

                if (_isSlicableObject)
                {
                    _gameParameters.ChangeHealth(-1);
                }
            }
        }
    }
}