using Cysharp.Threading.Tasks;
using Runtime.Infrastructure.AssetProvider;
using Runtime.SlicableObjects;
using UnityEngine;

namespace Runtime.Infrastructure.Factories
{
    public sealed class WorldFactory : IWorldFactory
    {
        private const string PathToSlicableObjectView = "Prefabs/WorldObjects/SlicableObjects/SlicableObject";
        
        private readonly IAssetProvider _assetProvider;
        private SlicableObjectView _slicablePrefabView;
        
        public WorldFactory(IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
        }
        
        public async UniTask<SlicableObjectView> CreateSlicableObjectView(Transform parent)
        {
            if (_slicablePrefabView is null)
            {
                _slicablePrefabView = await _assetProvider.LoadObject<SlicableObjectView>(PathToSlicableObjectView);
            }

            SlicableObjectView slicableObjectView = Object.Instantiate(_slicablePrefabView, Vector3.zero, Quaternion.identity, parent);
            slicableObjectView.gameObject.SetActive(false);
            
            return slicableObjectView;
        }

        public async UniTask<TResult> CreateObject<TResult>(string path, Transform parent) where TResult : Object
        {
            TResult prefab = await _assetProvider.LoadObject<TResult>(path);

            return Object.Instantiate(prefab, Vector3.zero, Quaternion.identity, parent);
        }
    }
}