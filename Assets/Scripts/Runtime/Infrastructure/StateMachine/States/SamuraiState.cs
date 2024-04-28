using Runtime.Infrastructure.Factories;
using Runtime.Infrastructure.SlicableObjects;
using Runtime.Infrastructure.SlicableObjects.CollisionDetector;
using Runtime.Infrastructure.SlicableObjects.Spawner;
using Runtime.Infrastructure.Timer;
using Runtime.StaticData.Boosts;
using Runtime.UI.Screens;
using UnityEngine;
using Zenject;

namespace Runtime.Infrastructure.StateMachine.States
{
    public sealed class SamuraiState : IState
    {
        private readonly Transform _screenParent;
        private readonly SamuraiSettings _samuraiSettings;
        private readonly IUIFactory _uiFactory;
        private readonly DiContainer _diContainer;
        private readonly IStopwatchable _stopwatchable;
        private readonly IGameStateMachine _gameStateMachine;
        private readonly ICollisionDetector<Collider2D, SlicableObjectView> _collisionDetector;
        private readonly SlicableObjectSpawnerManager _slicableObjectSpawnerManager;

        private SamuraiScreen _samuraiScreen;
        
        public SamuraiState(
            Transform screenParent,
            SamuraiSettings samuraiSettings,
            IUIFactory uiFactory,
            DiContainer diContainer,
            IStopwatchable stopwatchable,
            IGameStateMachine gameStateMachine,
            ICollisionDetector<Collider2D, SlicableObjectView> collisionDetector,
            SlicableObjectSpawnerManager slicableObjectSpawnerManager)
        {
            _screenParent = screenParent;
            _samuraiSettings = samuraiSettings;
            _uiFactory = uiFactory;
            _diContainer = diContainer;
            _stopwatchable = stopwatchable;
            _gameStateMachine = gameStateMachine;
            _collisionDetector = collisionDetector;
            _slicableObjectSpawnerManager = slicableObjectSpawnerManager;

            _stopwatchable.TickEnded += OnTickEnded;
        }

        public void Enter()
        {
            _samuraiScreen = _uiFactory.LoadScreen<SamuraiScreen>(ScreenType.Samurai, _screenParent, _diContainer);

            _stopwatchable.Notch(_samuraiSettings.Duration);
            _collisionDetector.RemoveAllBoostColliders();
            _slicableObjectSpawnerManager.UpdateSpawnSettings(_samuraiSettings.FruitsMultiplier, _samuraiSettings.Duration, _samuraiSettings.TimeDivide, _samuraiSettings.SpawnOffsetDivide);
        }

        public void Exit()
        {
            Object.Destroy(_samuraiScreen.gameObject);
        }

        private void OnTickEnded()
        {
            _gameStateMachine.Enter<GameState>();
        }
    }
}