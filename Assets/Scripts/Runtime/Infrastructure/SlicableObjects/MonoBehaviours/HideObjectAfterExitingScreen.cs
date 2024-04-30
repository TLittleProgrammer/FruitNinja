using Cysharp.Threading.Tasks;
using Runtime.Infrastructure.Game;
using Runtime.Infrastructure.SlicableObjects.CollisionDetector;
using Runtime.Infrastructure.SlicableObjects.HideCondition;
using Runtime.Infrastructure.SlicableObjects.Movement;
using Runtime.Infrastructure.SlicableObjects.Services;
using Runtime.Infrastructure.StateMachine;
using Runtime.Infrastructure.StateMachine.States;
using UnityEngine;
using Zenject;

namespace Runtime.Infrastructure.SlicableObjects.MonoBehaviours
{
    public class HideObjectAfterExitingScreen : MonoBehaviour, IAsyncInitializable<IGameStateMachine, IMimikService>
    {
        [SerializeField] private bool _isSlicableObject = true;
        [SerializeField] private SlicableObjectView _slicableObjectView;
        
        private SlicableMovementService _slicableMovementService;
        private GameParameters _gameParameters;
        private IConditionObjectHideService _conditionObjectHideService;
        private GameScreenManager _gameScreenManager;
        private ISlicableObjectCounterOnMap _slicableObjectCounterOnMap;
        private ICollisionDetector<Collider2D, SlicableObjectView> _collisionDetector;
        private IGameStateMachine _gameStateMachine;
        private IMimikService _mimikService;

        [Inject]
        private void Construct(
            SlicableMovementService slicableMovementService,
            GameScreenManager gameScreenManager,
            GameParameters gameParameters,
            ISlicableObjectCounterOnMap slicableObjectCounterOnMap,
            ICollisionDetector<Collider2D, SlicableObjectView> collisionDetector)
        {
            _collisionDetector = collisionDetector;
            _slicableObjectCounterOnMap = slicableObjectCounterOnMap;
            _gameScreenManager = gameScreenManager;
            _gameParameters = gameParameters;
            _slicableMovementService = slicableMovementService;
        }

        public async UniTask AsyncInitialize(IGameStateMachine gameStateMachine, IMimikService mimikService)
        {
            _gameStateMachine = gameStateMachine;
            _mimikService = mimikService;

            await UniTask.CompletedTask;
        }

        private void LateUpdate()
        {
            //TODO Подумать над тем, чтобы перенести
            if (_conditionObjectHideService is null)
            {
                _conditionObjectHideService = new SimpleConditionHideObjectService(transform, _gameScreenManager.GetHorizontalSizeWithStep() + 1f, -_gameScreenManager.GetOrthographicSize() - 1f - 1.5f);
            }
            
            if (_conditionObjectHideService.IsNeedHideObject())
            {
                _mimikService.RemoveMimik(_slicableObjectView);
                _slicableMovementService.RemoveFromMapping(transform);
                _slicableObjectCounterOnMap.RemoveType(_slicableObjectView.SlicableObjectType);
                _collisionDetector.RemoveCollider(_slicableObjectView.Collider2D);

                if (_isSlicableObject &&
                    _slicableObjectView.SlicableObjectType is SlicableObjectType.Simple &&
                    _slicableObjectView.IsDamagable &&
                    _gameStateMachine.CurrentState is not SamuraiState)
                {
                    _gameParameters.ChangeHealth(-1);
                }
                
                gameObject.SetActive(false);
            }
        }
    }
}