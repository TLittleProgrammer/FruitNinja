using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Runtime.StaticData.UI;
using UnityEngine;

namespace Runtime.Infrastructure.SlicableObjects
{
    public class SlicableSpriteContainer : IAsyncInitializable
    {
        private readonly SlicableSpriteProvider _slicableSpriteProvider;
        
        private Dictionary<SlicableObjectType, List<Sprite>> _spritesDictionary;
        private Dictionary<string, List<Sprite>> _blotsDictionary;

        public SlicableSpriteContainer(SlicableSpriteProvider slicableSpriteProvider)
        {
            _slicableSpriteProvider = slicableSpriteProvider;
        }
        
        public async UniTask AsyncInitialize()
        {
            FillSpritesDictionaries();
            
            await UniTask.CompletedTask;
        }

        private void FillSpritesDictionaries()
        {
            _spritesDictionary = new();
            _blotsDictionary = new();

            foreach (SlicableDictionary slicableDictionary in _slicableSpriteProvider.SlicableDictionary)
            {
                foreach (SlicableItemParams slicableParams in slicableDictionary.SlicableItem.Params)
                {
                    AddItemToSpritesDictionary(slicableDictionary, slicableParams);
                    AddSpritesToBlotsList(slicableParams);
                }
            }
        }

        private void AddSpritesToBlotsList(SlicableItemParams slicableParams)
        {
            if (!_blotsDictionary.ContainsKey(slicableParams.Sprite.name))
            {
                _blotsDictionary.Add(slicableParams.Sprite.name, new());
            }

            _blotsDictionary[slicableParams.Sprite.name].AddRange(slicableParams.Blots);
        }

        private void AddItemToSpritesDictionary(SlicableDictionary slicableDictionary, SlicableItemParams slicableParams)
        {
            if (!_spritesDictionary.ContainsKey(slicableDictionary.SlicableObjectType))
            {
                _spritesDictionary.Add(slicableDictionary.SlicableObjectType, new());
            }

            _spritesDictionary[slicableDictionary.SlicableObjectType].Add(slicableParams.Sprite);
        }

        public Sprite GetRandomSprite(SlicableObjectType slicableObjectType)
        {
            if (_spritesDictionary.TryGetValue(slicableObjectType, out List<Sprite> sprites))
            {
                return sprites[Random.Range(0, sprites.Count)];
            }

            return null;
        }

        public Sprite GetRandomBlot(string name)
        {
            if (_blotsDictionary.TryGetValue(name, out List<Sprite> sprites))
            {
                if (sprites.Count == 0)
                    return null;
                
                return sprites[Random.Range(0, sprites.Count)];
            }

            return null;
        }
    }
}