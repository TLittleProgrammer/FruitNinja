using System;
using System.Linq;
using Runtime.Extensions;
using Runtime.Infrastructure.Effects;
using Runtime.Infrastructure.Mouse;
using Runtime.Infrastructure.SlicableObjects.Movement;
using UnityEngine;

namespace Runtime.Infrastructure.SlicableObjects
{
    public class CanSliceResolver
    {
        private readonly MouseManager _mouseManager;
        private readonly SlicableSpriteContainer _slicableSpriteContainer;
        private readonly SlicableMovementService _slicableMovementService;
        private readonly SliceableObjectDummy.Pool _dummyPool;
        private readonly BlotEffect.Pool _blotEffectPool;

        public CanSliceResolver(
            MouseManager mouseManager,
            SlicableSpriteContainer slicableSpriteContainer,
            SlicableMovementService slicableMovementService,
            SliceableObjectDummy.Pool dummyPool,
            BlotEffect.Pool blotEffectPool
        )
        {
            _mouseManager = mouseManager;
            _slicableSpriteContainer = slicableSpriteContainer;
            _slicableMovementService = slicableMovementService;
            _dummyPool = dummyPool;
            _blotEffectPool = blotEffectPool;
        }

        public void TrySlice(SlicableObjectView slicableObjectView)
        {
            if (_mouseManager.CanSlice)
            {
                Sprite slicableObjectSprite = slicableObjectView.MainSprite.sprite;
                Sprite sprite = GetSlicedSpriteByName(slicableObjectSprite);
                
                SliceableObjectDummy[] dummyArray = TakeDummies();
                
                dummyArray[0].ChangeSprite(sprite);
                dummyArray[1].ChangeSprite(sprite);
                
                SlicableModel slicableModel = _slicableMovementService.GetSliceableModel(slicableObjectView.transform);

                ChangeDummiesPosition(slicableModel, dummyArray, slicableObjectSprite);
                AddMappingToMovementService(slicableModel, dummyArray);

                _slicableMovementService.RemoveFromMapping(slicableObjectView.transform);
                slicableObjectView.gameObject.SetActive(false);

                AddBlotEffect(slicableObjectView.transform.position, slicableObjectSprite);
            }
        }

        private void AddBlotEffect(Vector2 targetPosition, Sprite sprite)
        {
            Sprite blotSprite = _slicableSpriteContainer.GetRandomBlot(sprite.name);

            if (blotSprite is not null)
            {
                BlotEffect blotEffect = _blotEffectPool.InactiveItems.First(_ => !_.gameObject.activeInHierarchy);
                
                blotEffect.Animate(targetPosition, blotSprite, () =>
                {
                    blotEffect.enabled = false;
                });
            }
        }

        private void AddMappingToMovementService(SlicableModel slicableModel, SliceableObjectDummy[] dummyArray)
        {
            SlicableModel modelFirstDummy = slicableModel.CreateCopy(dummyArray[0].transform, dummyArray[0].SlicableObjectView.ShadowSprite.transform);
            SlicableModel modelSecondDummy = slicableModel.CreateCopy(dummyArray[1].transform, dummyArray[1].SlicableObjectView.ShadowSprite.transform);

            _slicableMovementService.AddMapping(modelFirstDummy, dummyArray[0].transform);
            _slicableMovementService.AddMapping(modelSecondDummy, dummyArray[1].transform);
        }

        //TODO Исправить смещение половинок
        private static void ChangeDummiesPosition(SlicableModel slicableModel, SliceableObjectDummy[] dummyArray, Sprite sprite)
        {
            float offsetX = sprite.texture.height / sprite.pixelsPerUnit / 4f;
            float offsetY = sprite.texture.width / sprite.pixelsPerUnit / 4f;

            Vector2 direction = ((Vector2)(slicableModel.Rotation * Vector2.up) - slicableModel.Position).normalized;

            dummyArray[0].transform.localScale = Vector3.one;
            dummyArray[1].transform.localScale = new Vector3(-1f, 1f, 1f);

            dummyArray[0].transform.position = slicableModel.Position;
            dummyArray[1].transform.position = slicableModel.Position + new Vector2(offsetX, offsetY) * direction;

            dummyArray[0].transform.rotation = slicableModel.Rotation;
            dummyArray[1].transform.rotation = slicableModel.Rotation;

            dummyArray[0].gameObject.SetActive(true);
            dummyArray[1].gameObject.SetActive(true);
        }

        //TODO снова попробовать перенести хранение половинок в SpriteContainer, чтобы не пересчитывать спрайт постоянно
        private Sprite GetSlicedSpriteByName(Sprite sprite)
        {
            Texture2D texture2D = sprite.texture;
            Rect rect = new Rect(0f, 0f, texture2D.width / 2f, texture2D.height);
                    
            return Sprite.Create(texture2D, rect, new Vector2(0.5f, 0.5f));
        }

        private SliceableObjectDummy[] TakeDummies()
        {
            return _dummyPool
                .InactiveItems
                .Where(_ => !_.gameObject.activeInHierarchy)
                .Take(2)
                .ToArray();
        }
    }
}