using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Runtime.Infrastructure.AssetProvider;
using Runtime.UI.Screens;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

namespace Runtime.Infrastructure.Factories
{
    public sealed class UIFactory : IUIFactory
    {
        private readonly DiContainer _diContainer;
        private readonly ScreenContainer _screenContainer;
        private readonly IAssetProvider _assetProvider;

        public UIFactory(DiContainer diContainer, ScreenContainer screenContainer, IAssetProvider assetProvider)
        {
            _diContainer = diContainer;
            _screenContainer = screenContainer;
            _assetProvider = assetProvider;
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
            
            return screen;
        }

        public async UniTask<TResult> LoadUIObjectByPath<TResult>(string path, Transform parent) where TResult : Object
        {
            TResult prefab = await _assetProvider.LoadObject<TResult>(path);

            return _diContainer
                .InstantiatePrefab(prefab, Vector3.zero, Quaternion.identity, parent)
                .GetComponentInChildren<TResult>();
        }
    }
}