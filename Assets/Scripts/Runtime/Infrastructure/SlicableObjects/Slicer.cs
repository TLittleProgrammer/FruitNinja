using System.Linq;
using Runtime.Extensions;
using Runtime.Infrastructure.Effects;
using Runtime.Infrastructure.Mouse;
using Runtime.Infrastructure.Score;
using Runtime.Infrastructure.SlicableObjects.Movement;
using UnityEngine;

namespace Runtime.Infrastructure.SlicableObjects
{
    public class Slicer
    {
        private readonly MouseManager _mouseManager;
        private readonly SlicableVisualContainer _slicableVisualContainer;
        private readonly SlicableMovementService _slicableMovementService;
        private readonly SliceableObjectDummy.Pool _dummyPool;
        private readonly BlotEffect.Pool _blotEffectPool;
        private readonly SplashEffect.Pool _splashEffectPool;
        private readonly ScoreEffect.Pool _scoreEffectPool;
        private readonly SliceableObjectSpriteRendererOrderService _orderService;
        private readonly IShowEffectsService _showEffectsService;
        private readonly IAddScoreService _addScoreService;

        private int _lastScore;

        public Slicer(
            MouseManager mouseManager,
            SlicableVisualContainer slicableVisualContainer,
            SlicableMovementService slicableMovementService,
            SliceableObjectDummy.Pool dummyPool,
            ScoreEffect.Pool scoreEffectPool,
            SliceableObjectSpriteRendererOrderService orderService,
            IShowEffectsService showEffectsService,
            IAddScoreService addScoreService
        )
        {
            _mouseManager = mouseManager;
            _slicableVisualContainer = slicableVisualContainer;
            _slicableMovementService = slicableMovementService;
            _dummyPool = dummyPool;
            _scoreEffectPool = scoreEffectPool;
            _orderService = orderService;
            _showEffectsService = showEffectsService;
            _addScoreService = addScoreService;
        }

        public void SliceObject(SlicableObjectView slicableObjectView)
        {
            Sprite slicableObjectSprite = slicableObjectView.MainSprite.sprite;
            Sprite sprite = _slicableVisualContainer.GetSlicedSpriteByName(slicableObjectSprite.name);


            AddDummies(slicableObjectView, sprite, slicableObjectSprite);
            RemoveSlicableObjectFromMapping(slicableObjectView);

            int score = _addScoreService.Add();
            _showEffectsService.ShowEffects(slicableObjectView.transform.position, slicableObjectSprite, score);
        }

        private void RemoveSlicableObjectFromMapping(SlicableObjectView slicableObjectView)
        {
            _slicableMovementService.RemoveFromMapping(slicableObjectView.transform);
            slicableObjectView.gameObject.SetActive(false);
        }

        private void AddDummies(SlicableObjectView slicableObjectView, Sprite sprite, Sprite slicableObjectSprite)
        {
            SliceableObjectDummy[] dummyArray = TakeDummies();

            dummyArray[0].ChangeSprite(sprite);
            dummyArray[1].ChangeSprite(sprite);

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