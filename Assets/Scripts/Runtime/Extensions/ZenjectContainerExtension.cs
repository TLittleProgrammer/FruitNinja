using UnityEngine;
using Zenject;

namespace Runtime.Extensions
{
    public static class ZenjectContainerExtension
    {
        public static void BindPool<TInstance, TPool>(this DiContainer diContainer, int initialSize, TInstance prefab, string parentName)
            where TPool : IMemoryPool
            where TInstance : MonoBehaviour 
        {
            diContainer
                .BindMemoryPool<TInstance, TPool>()
                .WithInitialSize(initialSize)
                .FromComponentInNewPrefab(prefab)
                .UnderTransformGroup(parentName);

        }
    }
}