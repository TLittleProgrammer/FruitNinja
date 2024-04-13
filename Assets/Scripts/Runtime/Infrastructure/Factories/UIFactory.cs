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

            Transform screenTransform = screen.GetComponent<Transform>();
            screenTransform.SetAsFirstSibling();
            
            RectTransform screenRectTransform = screen.GetComponent<RectTransform>();
            screenRectTransform.offsetMin = Vector2.zero;
            screenRectTransform.offsetMax = Vector2.zero;
            screenRectTransform.localScale = Vector3.one;
            screenRectTransform.anchoredPosition3D = Vector3.zero;
            
            return screen;
        }

        public async UniTask<TResult> LoadUIObjectByPath<TResult>(string path, Transform parent, Vector3 position = default) where TResult : Object
        {
            TResult prefab = await _assetProvider.LoadObject<TResult>(path);

            TResult instance = _diContainer
                .InstantiatePrefab(prefab, position, Quaternion.identity, parent)
                .GetComponentInChildren<TResult>();
            
            return instance;
        }
    }
}