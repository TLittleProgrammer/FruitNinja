using Runtime.Infrastructure.SlicableObjects.Movement;
using UnityEngine;
using Zenject;

namespace Runtime.Infrastructure.SlicableObjects
{
    public class HideObjectAfterExitingScreen : MonoBehaviour
    {
        private SlicableMovementService _slicableMovementService;
        private GameScreenManager _gameScreenManager;
        private bool _metOnScreen = false;

        [Inject]
        private void Construct(SlicableMovementService slicableMovementService, GameScreenManager gameScreenManager)
        {
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
            
            if (objectAtScreen is false && _metOnScreen)
            {
                gameObject.SetActive(false);
                _slicableMovementService.RemoveFromMapping(transform);
                _metOnScreen = false;
            }
        }
    }
}