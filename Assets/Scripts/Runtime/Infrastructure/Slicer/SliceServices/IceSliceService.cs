﻿using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Runtime.Infrastructure.Effects;
using Runtime.Infrastructure.Factories;
using Runtime.Infrastructure.SlicableObjects.MonoBehaviours;
using Runtime.Infrastructure.SlicableObjects.Movement;
using Runtime.Infrastructure.Slicer.SliceServices.Helpers;
using Runtime.Infrastructure.StateMachine;
using Runtime.Infrastructure.StateMachine.States;
using Runtime.Infrastructure.Timer;
using Runtime.StaticData.Boosts;
using Runtime.UI.Screens;
using UnityEngine;
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
        private readonly BlurEffect _blurEffect;
        private readonly ITimeProvider _timeProvider;

        private GameObject _iceScreen;
        private bool _animated = false;

        private Sequence _sequence;

        public IceSliceService(
            Transform iceBackgroundParent,
            SlicableMovementService slicableMovementService,
            ICreateDummiesService createDummiesService,
            IceSettings iceSettings,
            IUIFactory uiFactory,
            DiContainer diContainer,
            BlurEffect blurEffect,
            ITimeProvider timeProvider,
            IGameStateMachine gameStateMachine) : base(slicableMovementService)
        {
            _iceBackgroundParent = iceBackgroundParent;
            _createDummiesService = createDummiesService;
            _iceSettings = iceSettings;
            _uiFactory = uiFactory;
            _diContainer = diContainer;
            _blurEffect = blurEffect;
            _timeProvider = timeProvider;
            
            gameStateMachine.UpdatedState += OnUpdatedState;
        }

        public override bool TrySlice(SlicableObjectView slicableObjectView)
        {
            if (_animated)
            {
                return true;
            }
            TryAnimate();
            _createDummiesService.AddDummies(slicableObjectView);
            RemoveSlicableObjectFromMapping(slicableObjectView);

            return true;
        }

        private void TryAnimate()
        {
            _blurEffect.UpdateBlur(1f, 0.5f).Forget();
            _iceScreen = _uiFactory.LoadScreen<RectTransform>(ScreenType.Ice, _iceBackgroundParent, _diContainer).gameObject;
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
            _sequence.AppendInterval(_iceSettings.Duration).ToUniTask().Forget();
            _sequence.Append(ChangeScale(_iceSettings.TargetTimeScale, 1f, _iceSettings.DurationToReturnNormalTimeScale, SetTimeScale)).ToUniTask().Forget();
            _sequence.OnComplete(() =>
            {
                _blurEffect.UpdateBlur(0f, 0.5f).Forget();
            
                _animated = false;
                Object.Destroy(_iceScreen);
                
                _sequence.Kill();
            });
        }

        private Tweener ChangeScale(float from, float target, float duration, Action<float> updated)
        {
            return DOVirtual.Float(from, target, duration, updated.Invoke);
        }

        private void SetTimeScale(float timeScale)
        {
            _timeProvider.SetScale(timeScale);
        }
    }
}