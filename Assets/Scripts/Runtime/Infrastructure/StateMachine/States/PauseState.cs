using Runtime.Infrastructure.Factories;
using Runtime.Infrastructure.Mouse;
using Runtime.Infrastructure.SlicableObjects.Movement;
using Runtime.Infrastructure.SlicableObjects.Spawner;
using Runtime.Infrastructure.Trail;
using Runtime.UI.Screens;
using UnityEngine;
using Zenject;

namespace Runtime.Infrastructure.StateMachine.States
{
    public sealed class PauseState : IState
    {
        private readonly Canvas _pauseScreenParent;
        private readonly IUIFactory _uiFactory;
        private readonly SlicableObjectSpawnerManager _spawnerManager;
        private readonly DiContainer _diContainer;
        private readonly SlicableMovementService _movementService;
        private readonly MouseManager _mouseManager;
        private readonly TrailMoveService _trailMoveService;
        
        private Object _pauseScreen;

        public PauseState(
            Canvas pauseScreenParent,
            IUIFactory uiFactory,
            DiContainer diContainer,
            SlicableObjectSpawnerManager spawnerManager,
            SlicableMovementService movementService,
            TrailMoveService trailMoveService,
            MouseManager mouseManager
        )
        {
            _pauseScreenParent = pauseScreenParent;
            _uiFactory = uiFactory;
            _diContainer = diContainer;
            _spawnerManager = spawnerManager;
            _movementService = movementService;
            _trailMoveService = trailMoveService;
            _mouseManager = mouseManager;
        }
        
        public void Enter()
        {
            _spawnerManager.SetStop(true);
            _mouseManager.SetStopValue(true);
            _trailMoveService.SetCanTrail(false);
            _movementService.SetCanMove(false);

            CreateLooseWindow();
        }

        public void Exit()
        {
            _spawnerManager.SetStop(false);
            _mouseManager.SetStopValue(false);
            _trailMoveService.SetCanTrail(true);
            _movementService.SetCanMove(true);
            
            Object.Destroy(_pauseScreen);
        }

        private void CreateLooseWindow()
        {
            _pauseScreen = _uiFactory.LoadScreen<PauseScreen>(ScreenType.Pause, _pauseScreenParent.transform, _diContainer).gameObject;
        }
    }
}