using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Runtime.Infrastructure.AssetProvider
{
    public interface IAssetProvider
    {
        UniTask<TResult> LoadObject<TResult>(string path) where TResult : Object;
    }

    public sealed class ResourcesAssetProvider : IAssetProvider
    {
        public async UniTask<TResult> LoadObject<TResult>(string path) where TResult : Object
        {
            return await Resources.LoadAsync<TResult>(path) as TResult;
        }
    }
}