using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Runtime.Infrastructure.SlicableObjects;
using Runtime.StaticData.UI;
using UnityEngine;

namespace Runtime.Infrastructure.Containers
{
    public sealed class SpriteProviderContainer : IAsyncInitializable
    {
        private readonly SpriteProvider _spriteProvider;
        private Dictionary<string, Sprite> _sprites;
        private Dictionary<SlicableObjectType, Sprite> _spritesByType;

        public SpriteProviderContainer(SpriteProvider spriteProvider)
        {
            _spriteProvider = spriteProvider;
        }

        public async UniTask AsyncInitialize()
        {
            _sprites = _spriteProvider
                .SlicableDictionary
                .ToDictionary(x => x.Id, x => x.Sprite);

            _spritesByType = _spriteProvider
                .IconsByType
                .ToDictionary(x => x.Id, x => x.Sprite);

            await UniTask.CompletedTask;
        }

        public Sprite GetSprite(string name) =>
            _sprites.TryGetValue(name, out Sprite sprite) ? sprite : null;
        
        public Sprite GetSpriteByType(SlicableObjectType type) =>
            _spritesByType.TryGetValue(type, out Sprite sprite) ? sprite : null;
    }
}