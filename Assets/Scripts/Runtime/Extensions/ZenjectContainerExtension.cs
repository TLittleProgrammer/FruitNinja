using UnityEngine;
using Zenject;

namespace Runtime.Extensions
{
    public static class ZenjectContainerExtension
    {
        public static void BindPool<TInstance, TPool>(this DiContainer diContainer, int initialSize, TInstance prefab, Transform parent)
            where TPool : IMemoryPool
            where TInstance : MonoBehaviour 
        {
            diContainer
                .BindMemoryPool<TInstance, TPool>()
                .WithInitialSize(initialSize)
                .FromComponentInNewPrefab(prefab)
                .UnderTransform(parent);
        }
    }
}