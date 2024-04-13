using Cysharp.Threading.Tasks;
using Runtime.SlicableObjects;
using UnityEngine;

namespace Runtime.Infrastructure.Factories
{
    public interface IWorldFactory
    {
        UniTask<SlicableObjectView> CreateSlicableObjectView(Transform parent);
        UniTask<TResult> CreateObject<TResult>(string path, Transform parent) where TResult : Object;
    }
}