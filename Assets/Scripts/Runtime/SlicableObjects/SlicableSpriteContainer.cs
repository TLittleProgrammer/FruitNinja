using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Runtime.Infrastructure;
using Runtime.Infrastructure.Bootstrap.ScriptableObjects;
using UnityEngine;

namespace Runtime.SlicableObjects
{
    public class SlicableSpriteContainer : IAsyncInitializable
    {
        private readonly SlicableSpriteProvider _slicableSpriteProvider;
        
        private Dictionary<SlicableObjectType,List<Sprite>> _spritesDictionary;

        public SlicableSpriteContainer(SlicableSpriteProvider slicableSpriteProvider)
        {
            _slicableSpriteProvider = slicableSpriteProvider;
        }
        
        public async UniTask AsyncInitialize()
        {
            _spritesDictionary = _slicableSpriteProvider
                .SlicableDictionary
                .ToDictionary(x => x.SlicableObjectType, x => x.SlicableItem.Sprites);

            await UniTask.CompletedTask;
        }

        public Sprite GetRandomSprite(SlicableObjectType slicableObjectType)
        {
            if (_spritesDictionary.TryGetValue(slicableObjectType, out List<Sprite> sprites))
            {
                return sprites[Random.Range(0, sprites.Count)];
            }

            return null;
        }
    }
}