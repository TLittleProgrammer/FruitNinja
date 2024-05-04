using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Runtime.Infrastructure.Effects;
using Runtime.Infrastructure.Factories;
using Runtime.Infrastructure.Mouse;
using Runtime.Infrastructure.SlicableObjects;
using Runtime.Infrastructure.SlicableObjects.MonoBehaviours;
using Runtime.Infrastructure.SlicableObjects.Movement;
using Runtime.Infrastructure.Slicer.SliceServices.Helpers;
using Runtime.Infrastructure.StateMachine;
using Runtime.Infrastructure.StateMachine.States;
using Runtime.Infrastructure.Timer;
using Runtime.StaticData.Boosts;
using Runtime.UI.Screens;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Object = UnityEngine.Object;

namespace Runtime.Infrastructure.Slicer.SliceServices
{
    public sealed class IceSliceService : SliceService
    {
        private readonly Transform _iceBackgroundParent;
        private readonly ICreateDummiesService _createDummiesService;
        private readonly IceSettings _iceSettings;
        private readonly IUIFactory _uiFactory;
        private readonly DiContainer _diContainer;
        private readonly ITimeProvider _timeProvider;
        private readonly IShowEffectsService _showEffectsService;
        private readonly MouseManager _mouseManager;

        private Image _iceScreen;
        private bool _animated = false;
        private Color _initialColor;
        private Sequence _hide;

        private Sequence _sequence;

        public IceSliceService(
            Transform iceBackgroundParent,
            SlicableMovementService slicableMovementService,
            ICreateDummiesService createDummiesService,
            IceSettings iceSettings,
            IUIFactory uiFactory,
            DiContainer diContainer,
            ITimeProvider timeProvider,
            IGameStateMachine gameStateMachine,
            IShowEffectsService showEffectsService,
            MouseManager mouseManager) : base(slicableMovementService)
        {
            _iceBackgroundParent = iceBackgroundParent;
            _createDummiesService = createDummiesService;
            _iceSettings = iceSettings;
            _uiFactory = uiFactory;
            _diContainer = diContainer;
            _timeProvider = timeProvider;
            _showEffectsService = showEffectsService;
            _mouseManager = mouseManager;

            gameStateMachine.UpdatedState += OnUpdatedState;
        }

        public override bool TrySlice(SlicableObjectView slicableObjectView)
        {
            if (_animated)
            {
                _sequence?.Kill();
                _sequence = DOTween.Sequence();
                
                _hide?.Kill();
                _iceScreen.DOColor(_initialColor, _iceSettings.DurationToReturnNormalTimeScale).From(_iceScreen.color);
            
                _sequence.Append(ChangeScale(_timeProvider.TimeScale, _iceSettings.TargetTimeScale, _iceSettings.DurationToReturnNormalTimeScale, SetTimeScale)).ToUniTask().Forget();
                _sequence
                    .AppendInterval(_iceSettings.Duration)
                    .ToUniTask()
                    .Forget();
                _sequence.Append(ChangeScale(_iceSettings.TargetTimeScale, 1f, _iceSettings.DurationToReturnNormalTimeScale, SetTimeScale)).ToUniTask().Forget();
                _sequence.OnComplete(() =>
                {
                    _animated = false;
                    _hide.Kill();
                    Object.Destroy(_iceScreen.gameObject);
                
                    _sequence.Kill();
                });
                
                HideBack(_iceSettings.Duration + _iceSettings.DurationToReturnNormalTimeScale * 2f);
                
                _createDummiesService.AddDummies(slicableObjectView);
                RemoveSlicableObjectFromMapping(slicableObjectView);
                return true;
            }
            TryAnimate();
            _createDummiesService.AddDummies(slicableObjectView);
            RemoveSlicableObjectFromMapping(slicableObjectView);
                        
            _showEffectsService.PlayHeartSplash(_mouseManager.GetScreenPosition(slicableObjectView.transform.position), SlicableObjectType.Ice);


            return true;
        }

        private void HideBack(float offset)
        {
            _hide?.Kill();
            _hide = DOTween.Sequence()
                .AppendInterval(offset)
                .Append(_iceScreen.DOColor(Color.clear, _iceSettings.DurationToReturnNormalTimeScale));
        }

        private void TryAnimate()
        {
            _iceScreen = _uiFactory.LoadScreen<Image>(ScreenType.Ice, _iceBackgroundParent, _diContainer);
            _initialColor = _iceScreen.color;
            
            _iceScreen.DOColor(_initialColor, _iceSettings.DurationToReturnNormalTimeScale).From(Color.clear);
            
            _animated = true;
            
            Animate();
        }

        private void OnUpdatedState(IExitableState state)
        {
            if (state is PauseState or LooseState)
            {
                _sequence.Pause();
            }
            else
            {
                _sequence.Play();
            }
        }

        private void Animate()
        {
            _sequence = DOTween.Sequence();
            
            _sequence.Append(ChangeScale(1f, _iceSettings.TargetTimeScale, _iceSettings.DurationToReturnNormalTimeScale, SetTimeScale)).ToUniTask().Forget();
            _sequence
                .AppendInterval(_iceSettings.Duration)
                .ToUniTask()
                .Forget();
            _sequence.Append(ChangeScale(_iceSettings.TargetTimeScale, 1f, _iceSettings.DurationToReturnNormalTimeScale, SetTimeScale)).ToUniTask().Forget();
            _sequence.OnComplete(() =>
            {
                _animated = false;
                _hide.Kill();
                Object.Destroy(_iceScreen.gameObject);
                
                _sequence.Kill();
            });

            HideBack(_iceSettings.DurationToReturnNormalTimeScale + _iceSettings.Duration);
        }

        private Tweener ChangeScale(float from, float target, float duration, Action<float> updated, bool flag = false)
        {
            return DOVirtual.Float(from, target, duration, updated.Invoke);
        }

        private void SetTimeScale(float timeScale)
        {
            _timeProvider.SetScale(timeScale);
        }
    }
}