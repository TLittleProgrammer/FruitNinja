using System.Linq;
using Runtime.Extensions;
using Runtime.Infrastructure.Containers;
using Runtime.Infrastructure.Mouse;
using Runtime.Infrastructure.SlicableObjects;
using Runtime.Infrastructure.SlicableObjects.MonoBehaviours;
using Runtime.Infrastructure.SlicableObjects.Movement;
using Runtime.StaticData.UI;
using UnityEngine;

namespace Runtime.Infrastructure.Slicer.SliceServices.Helpers
{
    public sealed class CreateDummiesService : ICreateDummiesService
    {
        private readonly MouseManager _mouseManager;
        private readonly SliceableObjectDummy.Pool _dummyPool;
        private readonly SliceableObjectSpriteRendererOrderService _orderService;
        private readonly SlicableMovementService _slicableMovementService;
        private readonly SlicableVisualContainer _slicableVisualContainer;
        private readonly SpriteProviderContainer _spriteProviderContainer;
        private readonly SpriteProvider _spriteProvider;

        public CreateDummiesService(
            MouseManager mouseManager,
            SliceableObjectDummy.Pool dummyPool,
            SliceableObjectSpriteRendererOrderService orderService,
            SlicableMovementService slicableMovementService,
            SlicableVisualContainer slicableVisualContainer,
            SpriteProviderContainer spriteProviderContainer
        )
        {
            _mouseManager = mouseManager;
            _dummyPool = dummyPool;
            _orderService = orderService;
            _slicableMovementService = slicableMovementService;
            _slicableVisualContainer = slicableVisualContainer;
            _spriteProviderContainer = spriteProviderContainer;
        }
        
        public void AddDummies(SlicableObjectView slicableObjectView)
        {
            Sprite slicableObjectSprite = slicableObjectView.MainSprite.sprite;
            Sprite slicedSprite = _slicableVisualContainer.GetSlicedSpriteByName(slicableObjectSprite.name);
            
            SliceableObjectDummy[] dummyArray = TakeDummies();

            dummyArray[0].ChangeSprite(slicedSprite);
            dummyArray[1].ChangeSprite(slicedSprite);

            UpdateSortingInLayerIndex(dummyArray);

            SlicableModel slicableModel = _slicableMovementService.GetSliceableModel(slicableObjectView.transform);

            ChangeDummiesPosition(slicableModel, dummyArray, slicableObjectSprite);
            AddMappingToMovementService(slicableModel, dummyArray);
        }
        
        public void AddDummies(SlicableObjectView slicableObjectView, string firstSpriteName, string secondSpriteName)
        {
            Sprite slicableObjectSprite = slicableObjectView.MainSprite.sprite;
            Sprite firstSprite = _spriteProviderContainer.GetSprite(firstSpriteName);
            Sprite secondSprite = _spriteProviderContainer.GetSprite(secondSpriteName);
            
            SliceableObjectDummy[] dummyArray = TakeDummies();

            dummyArray[0].ChangeSprite(firstSprite);
            dummyArray[1].ChangeSprite(secondSprite);

            UpdateSortingInLayerIndex(dummyArray);

            SlicableModel slicableModel = _slicableMovementService.GetSliceableModel(slicableObjectView.transform);

            ChangeDummiesPosition(slicableModel, dummyArray, slicableObjectSprite);
            AddMappingToMovementService(slicableModel, dummyArray);
        }
        
        private void UpdateSortingInLayerIndex(SliceableObjectDummy[] dummyArray)
        {
            _orderService.UpdateOrderInLayer(dummyArray[0].SlicableObjectView.MainSprite);
            _orderService.UpdateOrderInLayer(dummyArray[0].SlicableObjectView.ShadowSprite);
            _orderService.UpdateOrderInLayer(dummyArray[1].SlicableObjectView.MainSprite);
            _orderService.UpdateOrderInLayer(dummyArray[1].SlicableObjectView.ShadowSprite);
        }
        
        private void AddMappingToMovementService(SlicableModel slicableModel, SliceableObjectDummy[] dummyArray)
        {
            SlicableModel modelFirstDummy  = slicableModel.CreateCopy(dummyArray[0].transform, dummyArray[0].SlicableObjectView.ShadowSprite.transform);
            SlicableModel modelSecondDummy = slicableModel.CreateCopy(dummyArray[1].transform, dummyArray[1].SlicableObjectView.ShadowSprite.transform);

            SlicableModelParams firstParams  = modelFirstDummy.GetParams();
            SlicableModelParams secondParams = modelSecondDummy.GetParams();

            Vector2 mouseDirection = _mouseManager.GetMouseNormalizedDirection();
            float angleBetweenMouseDirectionAndVectorRight = Vector2.Angle(Vector2.right, mouseDirection);
            
            float firstAngle  = (angleBetweenMouseDirectionAndVectorRight + Random.Range(15f, 25f)).ConvertToRadians();
            float secondAngle = (angleBetweenMouseDirectionAndVectorRight - Random.Range(15f, 25f)).ConvertToRadians();
            
            modelFirstDummy.ResetMovementObjectService(firstParams.VelocityX + 0.25f, firstParams.VelocityY + 0.5f, firstAngle, dummyArray[0].transform.position);
            modelSecondDummy.ResetMovementObjectService(secondParams.VelocityX + 0.25f, secondParams.VelocityY + 0.5f, secondAngle, dummyArray[1].transform.position);
            
            _slicableMovementService.AddMapping(modelFirstDummy, dummyArray[0].transform);
            _slicableMovementService.AddMapping(modelSecondDummy, dummyArray[1].transform);
        }
        
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