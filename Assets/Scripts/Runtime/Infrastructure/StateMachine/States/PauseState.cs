using Cysharp.Threading.Tasks;
using DG.Tweening;
using Runtime.Infrastructure.Effects;
using Runtime.Infrastructure.Factories;
using Runtime.Infrastructure.Mouse;
using Runtime.Infrastructure.SlicableObjects.Movement;
using Runtime.Infrastructure.SlicableObjects.Services;
using Runtime.Infrastructure.SlicableObjects.Spawner;
using Runtime.Infrastructure.Slicer.SliceServices;
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
        private readonly IMimikService _mimikService;
        private readonly MagnetSliceService _magnetSliceService;
        private readonly BlurEffect _blurEffect;
        private readonly TrailMoveService _trailMoveService;
        
        private PauseScreen _pauseScreen;

        public PauseState(
            Canvas pauseScreenParent,
            IUIFactory uiFactory,
            DiContainer diContainer,
            SlicableObjectSpawnerManager spawnerManager,
            SlicableMovementService movementService,
            TrailMoveService trailMoveService,
            MouseManager mouseManager,
            IMimikService mimikService,
            MagnetSliceService magnetSliceService,
            BlurEffect blurEffect
        )
        {
            _pauseScreenParent = pauseScreenParent;
            _uiFactory = uiFactory;
            _diContainer = diContainer;
            _spawnerManager = spawnerManager;
            _movementService = movementService;
            _trailMoveService = trailMoveService;
            _mouseManager = mouseManager;
            _mimikService = mimikService;
            _magnetSliceService = magnetSliceService;
            _blurEffect = blurEffect;
        }
        
        public void Enter()
        {
            _spawnerManager.SetStop(true);
            _mouseManager.SetStopValue(true);
            _trailMoveService.SetCanTrail(false);
            _movementService.SetCanMove(false);
            _mimikService.SetSimulateSpeedToParticles(0f);
            _magnetSliceService.StopSequences();

            CreatePauseWindow();
        }

        public async void Exit()
        {
            await AnimatePauseScreen(Vector3.one, Vector3.zero, _pauseScreen.Background.color, Color.clear, false);
            
            _mimikService.SetSimulateSpeedToParticles(1f);
            _spawnerManager.SetStop(false);
            _mouseManager.SetStopValue(false);
            _trailMoveService.SetCanTrail(true);
            _movementService.SetCanMove(true);
            _magnetSliceService.PlaySequences();
            
            Object.Destroy(_pauseScreen.gameObject);
        }

        private async void CreatePauseWindow()
        {
            _pauseScreen = _uiFactory.LoadScreen<PauseScreen>(ScreenType.Pause, _pauseScreenParent.transform, _diContainer);

            Color targetColor = _pauseScreen.Background.color;
            await AnimatePauseScreen(Vector3.zero, Vector3.one, new(targetColor.r, targetColor.g, targetColor.b, 0f), targetColor, true);
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private async UniTask AnimatePauseScreen(Vector3 initialScale, Vector3 targetScale, Color initialColor, Color targetColor, bool animateBackBeforeTransform)
        {
            Sequence sequence = DOTween.Sequence();
            
            _pauseScreen.Background.color = initialColor;

            if (animateBackBeforeTransform)
            {
                await sequence.Append(_pauseScreen.Background.DOColor(targetColor, 0.5f));
                await _blurEffect.UpdateBlur(2f, 0.9f);
            }
            
            foreach (Transform transform in _pauseScreen.Transforms)
            {
                transform.localScale = initialScale;
                await sequence.Append(transform.DOScale(targetScale, 0.15f).SetEase(Ease.InCubic));
            }

            if (!animateBackBeforeTransform)
            {
                await sequence.Append(_pauseScreen.Background.DOColor(targetColor, 0.5f));
                await _blurEffect.UpdateBlur(0f, 0.9f);
            }

            await sequence.OnComplete(() =>
            {
                sequence.Kill();
            });

            await sequence.Play().ToUniTask();
        }
    }
}