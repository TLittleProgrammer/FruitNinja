using Cysharp.Threading.Tasks;
using DG.Tweening;
using Runtime.Infrastructure.Effects;
using Runtime.Infrastructure.Factories;
using Runtime.Infrastructure.Mouse;
using Runtime.Infrastructure.SlicableObjects.Movement;
using Runtime.Infrastructure.SlicableObjects.Spawner;
using Runtime.Infrastructure.Slicer.SliceServices;
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
        private readonly MagnetSliceService _magnetSliceService;
        private readonly BlurEffect _blurEffect;

        private bool _isEntered;
        private LooseScreen _looseScreen;
        
        public LooseState(
            Canvas looseScreenParent,
            IUIFactory uiFactory,
            DiContainer diContainer,
            SlicableObjectSpawnerManager spawnerManager,
            SlicableMovementService movementService,
            TrailMoveService trailMoveService,
            MouseManager mouseManager,
            MagnetSliceService magnetSliceService,
            BlurEffect blurEffect
            )
        {
            _looseScreenParent = looseScreenParent;
            _uiFactory = uiFactory;
            _diContainer = diContainer;
            _spawnerManager = spawnerManager;
            _movementService = movementService;
            _trailMoveService = trailMoveService;
            _mouseManager = mouseManager;
            _magnetSliceService = magnetSliceService;
            _blurEffect = blurEffect;
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
            _magnetSliceService.StopSequences();
            
            _isEntered = true;
        }

        public async void Exit()
        {
            if (_looseScreen is null)
                return;
            
            await HideLooseScreen();
            
            _spawnerManager.SetStop(false);
            _mouseManager.SetStopValue(false);
            _trailMoveService.SetCanTrail(true);
            _magnetSliceService.PlaySequences();
            
            Object.Destroy(_looseScreen.gameObject);

            _looseScreen = null;
            _isEntered = false;
        }

        private void CreateLooseWindow()
        {
            _looseScreen = _uiFactory.LoadScreen<LooseScreen>(ScreenType.Loose, _looseScreenParent.transform, _diContainer);

            ShowLooseScreen();
        }

        private void ShowLooseScreen()
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

            _blurEffect.UpdateBlur(2f, 0.75f);

            sequence.Play();
        }
        
        private UniTask HideLooseScreen()
        {
            Sequence sequence = DOTween.Sequence();
            
            _looseScreen.AllInfo.localScale = Vector3.one;
            sequence.Append(_looseScreen.AllInfo.DOScale(Vector3.zero, 0.15f).SetEase(Ease.InCubic));
            
            sequence.Append(_looseScreen.Background.DOColor(Color.clear, 0.5f));
            sequence.OnComplete(() =>
            {
                sequence.Kill();
            });
            
            _blurEffect.UpdateBlur(0f, 0.75f);

            return sequence.Play().ToUniTask();
        }
    }
}