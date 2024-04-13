using System;
using System.Linq;
using Runtime.Extensions;
using Runtime.Infrastructure.Mouse;
using Runtime.SlicableObjects.Movement;
using UnityEngine;

namespace Runtime.SlicableObjects
{
    public class CanSliceResolver
    {
        private readonly MouseManager _mouseManager;
        private readonly SlicableMovementService _slicableMovementService;
        private readonly SliceableObjectDummy.Pool _dummyPool;

        public event Action SlicedObjectView;
        
        public CanSliceResolver(
            MouseManager mouseManager,
            SlicableSpriteContainer slicableSpriteContainer,
            SlicableMovementService slicableMovementService,
            SliceableObjectDummy.Pool dummyPool
            )
        {
            _mouseManager = mouseManager;
            _slicableMovementService = slicableMovementService;
            _dummyPool = dummyPool;
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

                SlicableModel modelFirstDummy = slicableModel.CreateCopy(dummyArray[0].transform.position);
                SlicableModel modelSecondDummy = slicableModel.CreateCopy(dummyArray[1].transform.position);

                _slicableMovementService.AddMapping(modelFirstDummy, dummyArray[0].transform);
                _slicableMovementService.AddMapping(modelSecondDummy, dummyArray[1].transform);
                
                _slicableMovementService.RemoveFromMapping(slicableObjectView.transform);
                slicableObjectView.gameObject.SetActive(false);
            }
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