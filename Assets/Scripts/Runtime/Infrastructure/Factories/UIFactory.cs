using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Runtime.Infrastructure.AssetProvider;
using Runtime.Infrastructure.Bootstrap.ScriptableObjects;
using Runtime.UI.Screens;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Runtime.Infrastructure.Factories
{
    public sealed class UIFactory : IUIFactory
    {
        private readonly DiContainer _diContainer;
        private readonly ScreenContainer _screenContainer;
        private readonly IAssetProvider _assetProvider;
        private readonly ButtonAnimationSettings _buttonAnimationSettings;

        public UIFactory(DiContainer diContainer, ScreenContainer screenContainer, IAssetProvider assetProvider, ButtonAnimationSettings buttonAnimationSettings)
        {
            _diContainer = diContainer;
            _screenContainer = screenContainer;
            _assetProvider = assetProvider;
            _buttonAnimationSettings = buttonAnimationSettings;
        }
        
        public TResult LoadScreen<TResult>(ScreenType screenType, Transform parent) where TResult : Object
        {
            GameObject screenPrefab = _screenContainer.GetScreen(screenType);
            
            TResult screen = _diContainer
                .InstantiatePrefab(screenPrefab, Vector3.zero, Quaternion.identity, parent)
                .GetComponentInChildren<TResult>();

            screen.GetComponent<Transform>().SetAsFirstSibling();
            
            RectTransform screenRectTransform = screen.GetComponent<RectTransform>();
            screenRectTransform.offsetMin = Vector2.zero;
            screenRectTransform.offsetMax = Vector2.zero;
            screenRectTransform.localScale = Vector3.one;

            SubscribeButtonsClickOnAnimation(screen);
            
            return screen;
        }

        public async UniTask<TResult> LoadUIObjectByPath<TResult>(string path, Transform parent) where TResult : Object
        {
            TResult prefab = await _assetProvider.LoadObject<TResult>(path);

            TResult instance = _diContainer
                .InstantiatePrefab(prefab, Vector3.zero, Quaternion.identity, parent)
                .GetComponentInChildren<TResult>();
            
            SubscribeButtonsClickOnAnimation(instance);
            
            return instance;
        }

        private void SubscribeButtonsClickOnAnimation(Object screen)
        {
            Button[] buttons = screen.GetComponentsInChildren<Button>().ToArray();

            foreach (Button button in buttons)
            {
                Button needButton = button;
                
                needButton.onClick.AddListener(() =>
                {
                    needButton
                        .transform
                        .DOScale(_buttonAnimationSettings.TargetScale, _buttonAnimationSettings.Duration)
                        .SetEase(_buttonAnimationSettings.Ease)
                        .SetLoops(2, LoopType.Yoyo);
                });
            }
        }
    }
}