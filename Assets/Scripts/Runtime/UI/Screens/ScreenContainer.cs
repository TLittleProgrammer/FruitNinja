using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Runtime.Infrastructure;
using Runtime.Infrastructure.AssetProvider;
using UnityEngine;

namespace Runtime.UI.Screens
{
    public sealed class ScreenContainer : IAsyncInitializable
    {
        private const string PathToScreenProvider = "ScriptableObjects/ScreenProvider";
        
        private readonly IAssetProvider _assetProvider;
        
        private Dictionary<ScreenType,GameObject> _screens = new();

        public ScreenContainer(IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
        }
        
        public async UniTask AsyncInitialize()
        {
            _screens =
                (await _assetProvider.LoadObject<ScreenProvider>(PathToScreenProvider))
                .Screens
                .ToDictionary(x => x.ScreenType, x => x.GameObject);
        }

        public GameObject GetScreen(ScreenType screenType) =>
            _screens.TryGetValue(screenType, out GameObject prefab) ? prefab : null;
    }
}