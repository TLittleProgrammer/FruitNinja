using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Runtime.Infrastructure.Factories
{
    public interface IWorldFactory
    {
        UniTask<TResult> CreateObject<TResult>(string path, Transform parent) where TResult : Object;
    }
}