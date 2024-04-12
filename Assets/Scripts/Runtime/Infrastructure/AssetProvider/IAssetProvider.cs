using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Runtime.Infrastructure.AssetProvider
{
    public interface IAssetProvider
    {
        UniTask<TResult> LoadObject<TResult>(string path) where TResult : Object;
    }
}