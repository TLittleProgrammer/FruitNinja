using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Runtime.Extensions;
using Runtime.StaticData.UI;
using UnityEngine;

namespace Runtime.Infrastructure.SlicableObjects
{
    public class SlicableVisualContainer : IAsyncInitializable
    {
        private readonly SlicableSpriteProvider _slicableSpriteProvider;
        
        private Dictionary<SlicableObjectType, List<Sprite>> _spritesDictionary;
        private Dictionary<string, List<Sprite>> _blotsDictionary;
        private Dictionary<string, Sprite> _slicedSpritedDictionary;
        private Dictionary<string, Color> _splashColorDictionary;

        public SlicableVisualContainer(SlicableSpriteProvider slicableSpriteProvider)
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
            _spritesDictionary       = new();
            _blotsDictionary         = new();
            _splashColorDictionary   = new();
            _slicedSpritedDictionary = new();

            foreach (SlicableDictionary slicableDictionary in _slicableSpriteProvider.SlicableDictionary)
            {
                foreach (SlicableItemParams slicableParams in slicableDictionary.SlicableItem.Params)
                {
                    AddItemToSpritesDictionary(slicableDictionary, slicableParams);
                    AddSpritesToBlotsList(slicableParams);
                    AddColorToDictionary(slicableParams);
                    AddSlicedSpriteToDictionary(slicableParams);
                }
            }
        }

        public Sprite GetSlicedSpriteByName(string name) =>
            _slicedSpritedDictionary.TryGetValue(name, out Sprite slicedSprite) ? slicedSprite : null;

        public Color GetSplashColorBySpriteName(string spriteName) =>
            _splashColorDictionary.TryGetValue(spriteName, out Color color) ? color : Color.black;

        private void AddItemToSpritesDictionary(SlicableDictionary slicableDictionary, SlicableItemParams slicableParams)
        {
            SlicableObjectType slicableObjectType = slicableDictionary.SlicableObjectType;
            
            _spritesDictionary.AddItemWhereValueIsList(slicableObjectType, slicableParams.Sprite);
        }

        public Sprite GetRandomBlot(string name)
        {
            if (_blotsDictionary.TryGetValue(name, out List<Sprite> sprites))
            {
                return sprites.GetRandomValue();
            }

            return null;
        }

        public Sprite GetRandomSprite(SlicableObjectType slicableObjectType)
        {
            if (_spritesDictionary.TryGetValue(slicableObjectType, out List<Sprite> sprites))
            {
                return sprites.GetRandomValue();
            }

            return null;
        }

        private void AddColorToDictionary(SlicableItemParams slicableParams)
        {
            string spriteName = slicableParams.Sprite.name;
            
            if (!_splashColorDictionary.ContainsKey(spriteName))
            {
                _splashColorDictionary.Add(spriteName, slicableParams.SplashColor);
            }
        }

        private void AddSpritesToBlotsList(SlicableItemParams slicableParams)
        {
            string spriteName = slicableParams.Sprite.name;
            
            _blotsDictionary.AddItemWhereValueIsList(spriteName, slicableParams.Blots);
        }

        private void AddSlicedSpriteToDictionary(SlicableItemParams slicableParams)
        {
            Texture2D texture2D = slicableParams.Sprite.texture;
            Rect rect = new Rect(0f, 0f, texture2D.width / 2f, texture2D.height);
                    
            _slicedSpritedDictionary.Add(slicableParams.Sprite.name, Sprite.Create(texture2D, rect, new Vector2(0.5f, 0.5f)));
        }
    }
}