using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Runtime.Infrastructure.Effects;
using Runtime.Infrastructure.Factories;
using Runtime.Infrastructure.SlicableObjects.MonoBehaviours;
using Runtime.Infrastructure.SlicableObjects.Movement;
using Runtime.Infrastructure.Slicer.SliceServices.Helpers;
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

        private GameObject _iceScreen;
        private bool _animated = false;

        public IceSliceService(
            Transform iceBackgroundParent,
            SlicableMovementService slicableMovementService,
            ICreateDummiesService createDummiesService,
            IceSettings iceSettings,
            IUIFactory uiFactory,
            DiContainer diContainer,
            BlurEffect blurEffect) : base(slicableMovementService)
        {
            _iceBackgroundParent = iceBackgroundParent;
            _createDummiesService = createDummiesService;
            _iceSettings = iceSettings;
            _uiFactory = uiFactory;
            _diContainer = diContainer;
            _blurEffect = blurEffect;
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

        private async void TryAnimate()
        {
            _blurEffect.UpdateBlur(1f, 0.5f).Forget();
            _iceScreen = _uiFactory.LoadScreen<RectTransform>(ScreenType.Ice, _iceBackgroundParent, _diContainer).gameObject;
            _animated = true;
            
            await Animate();
            
            _blurEffect.UpdateBlur(0f, 0.5f).Forget();
            
            _animated = false;
            Object.Destroy(_iceScreen);
        }

        private async UniTask Animate()
        {
            await ChangeScale(1f, _iceSettings.TargetTimeScale, _iceSettings.DurationToReturnNormalTimeScale, SetTimeScale);
            await UniTask.Delay((int)(_iceSettings.Duration * 1000));
            await ChangeScale(_iceSettings.TargetTimeScale, 1f, _iceSettings.DurationToReturnNormalTimeScale, SetTimeScale);
        }

        private async UniTask ChangeScale(float from, float target, float duration, Action<float> updated)
        {
            await DOVirtual.Float(from, target, duration, updated.Invoke).ToUniTask();
        }

        private void SetTimeScale(float timeScale)
        {
            Time.timeScale = timeScale;
        }
    }
}