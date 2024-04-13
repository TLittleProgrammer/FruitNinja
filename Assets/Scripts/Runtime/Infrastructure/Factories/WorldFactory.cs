using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Runtime.Infrastructure.AssetProvider;
using Runtime.Infrastructure.SlicableObjects;
using Unity.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Runtime.Infrastructure.Factories
{
    public sealed class WorldFactory : IWorldFactory
    {
        private const string PathToSlicableObjectView = "Prefabs/WorldObjects/SlicableObjects/SlicableObject";
        
        private readonly IAssetProvider _assetProvider;
        private SlicableObjectView _slicablePrefabView;

        private readonly Dictionary<Type, Object> _prefabsDictioanry;

        public WorldFactory(IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
            _prefabsDictioanry = new();
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
            if (_prefabsDictioanry.TryGetValue(typeof(TResult), out Object result))
            {
                return result.GetComponentInChildren<TResult>();
            }
            
            TResult prefab = await _assetProvider.LoadObject<TResult>(path);

            _prefabsDictioanry.Add(typeof(TResult), prefab);
            
            return Object.Instantiate(prefab, Vector3.zero, Quaternion.identity, parent);
        }
    }
}