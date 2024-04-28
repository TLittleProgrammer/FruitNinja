using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Runtime.StaticData.UI;
using UnityEngine;

namespace Runtime.Infrastructure.Containers
{
    public sealed class SpriteProviderContainer : IAsyncInitializable
    {
        private readonly SpriteProvider _spriteProvider;
        private Dictionary<string, Sprite> _sprites;

        public SpriteProviderContainer(SpriteProvider spriteProvider)
        {
            _spriteProvider = spriteProvider;
        }

        public async UniTask AsyncInitialize()
        {
            _sprites = _spriteProvider
                .SlicableDictionary
                .ToDictionary(x => x.Id, x => x.Sprite);

            await UniTask.CompletedTask;
        }

        public Sprite GetSprite(string name) =>
            _sprites.TryGetValue(name, out Sprite sprite) ? sprite : null;
    }
}