using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Runtime.Infrastructure.AssetProvider;
using Unity.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Runtime.Infrastructure.Factories
{
    public sealed class WorldFactory : IWorldFactory
    {
        private readonly IAssetProvider _assetProvider;
        private readonly Dictionary<Type, Object> _prefabsDictionary;

        public WorldFactory(IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
            _prefabsDictionary = new();
        }

        public async UniTask<TResult> CreateObject<TResult>(string path, Transform parent) where TResult : Object
        {
            if (_prefabsDictionary.TryGetValue(typeof(TResult), out Object result))
            {
                return result.GetComponentInChildren<TResult>();
            }
            
            TResult prefab = await _assetProvider.LoadObject<TResult>(path);

            _prefabsDictionary.Add(typeof(TResult), prefab);
            
            return Object.Instantiate(prefab, Vector3.zero, Quaternion.identity, parent);
        }
    }
}