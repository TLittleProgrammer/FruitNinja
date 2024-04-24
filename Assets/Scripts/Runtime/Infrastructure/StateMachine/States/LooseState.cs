using Cysharp.Threading.Tasks;
using DG.Tweening;
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
    public sealed class LooseState : IState, ITickable
    {
        private readonly Canvas _looseScreenParent;
        private readonly IUIFactory _uiFactory;
        private readonly DiContainer _diContainer;
        private readonly SlicableObjectSpawnerManager _spawnerManager;
        private readonly SlicableMovementService _movementService;
        private readonly TrailMoveService _trailMoveService;
        private readonly MouseManager _mouseManager;

        private bool _isEntered;
        private LooseScreen _looseScreen;
        
        public LooseState(
            Canvas looseScreenParent,
            IUIFactory uiFactory,
            DiContainer diContainer,
            SlicableObjectSpawnerManager spawnerManager,
            SlicableMovementService movementService,
            TrailMoveService trailMoveService,
            MouseManager mouseManager
            )
        {
            _looseScreenParent = looseScreenParent;
            _uiFactory = uiFactory;
            _diContainer = diContainer;
            _spawnerManager = spawnerManager;
            _movementService = movementService;
            _trailMoveService = trailMoveService;
            _mouseManager = mouseManager;
            _isEntered = false;
        }

        public void Tick()
        {
            if (_isEntered is false)
                return;
            
            if (_movementService.MovementObjectsCount == 0 && _looseScreen is null)
            {
                CreateLooseWindow();
            }
        }

        public void Enter()
        {
            _spawnerManager.SetStop(true);
            _mouseManager.SetStopValue(true);
            _trailMoveService.SetCanTrail(false);
            
            _isEntered = true;
        }

        public async void Exit()
        {
            await HideLooseScreen();
            
            _spawnerManager.SetStop(false);
            _mouseManager.SetStopValue(false);
            _trailMoveService.SetCanTrail(true);
            
            Object.Destroy(_looseScreen.gameObject);

            _looseScreen = null;
            _isEntered = false;
        }

        private async void CreateLooseWindow()
        {
            _looseScreen = _uiFactory.LoadScreen<LooseScreen>(ScreenType.Loose, _looseScreenParent.transform, _diContainer);

            await ShowLooseScreen();
        }

        private async UniTask ShowLooseScreen()
        {
            Sequence sequence = DOTween.Sequence();

            Color initialColor = _looseScreen.Background.color;
            _looseScreen.Background.color = Color.clear;

            sequence.Append(_looseScreen.Background.DOColor(initialColor, 0.5f));

            _looseScreen.AllInfo.localScale = Vector3.zero;
            sequence.Append(_looseScreen.AllInfo.DOScale(Vector3.one, 0.15f).SetEase(Ease.InCubic));

            sequence.OnComplete(() =>
            {
                sequence.Kill();
            });

            await sequence.Play().ToUniTask();
        }
        
        private async UniTask HideLooseScreen()
        {
            Sequence sequence = DOTween.Sequence();
            
            _looseScreen.AllInfo.localScale = Vector3.one;
            sequence.Append(_looseScreen.AllInfo.DOScale(Vector3.zero, 0.15f).SetEase(Ease.InCubic));
            
            sequence.Append(_looseScreen.Background.DOColor(Color.clear, 0.5f));

            sequence.OnComplete(() =>
            {
                sequence.Kill();
            });

            await sequence.Play().ToUniTask();
        }
    }
}