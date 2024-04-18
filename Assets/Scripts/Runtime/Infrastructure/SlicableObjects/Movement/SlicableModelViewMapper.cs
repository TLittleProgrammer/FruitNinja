using System;
using Runtime.Constants;
using Runtime.Extensions;
using Runtime.Infrastructure.SlicableObjects.Movement.Animation;
using Runtime.Infrastructure.SlicableObjects.Spawner;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Runtime.Infrastructure.SlicableObjects.Movement
{
    public class SlicableModelViewMapper
    {
        private readonly SlicableObjectView.Pool _objectPool;
        private readonly SlicableVisualContainer _slicableVisualContainer;
        private readonly GameScreenManager _gameScreenManager;
        private readonly SlicableMovementService _slicableMovementService;
        private readonly SliceableObjectSpriteRendererOrderService _orderService;
        private readonly CollisionDetector.CollisionDetector _collisionDetector;

        public SlicableModelViewMapper(
            SlicableObjectView.Pool objectPool,
            SlicableVisualContainer slicableVisualContainer,
            GameScreenManager gameScreenManager,
            SlicableMovementService slicableMovementService,
            SliceableObjectSpriteRendererOrderService orderService,
            CollisionDetector.CollisionDetector collisionDetector)
        {
            _objectPool = objectPool;
            _slicableVisualContainer = slicableVisualContainer;
            _gameScreenManager = gameScreenManager;
            _slicableMovementService = slicableMovementService;
            _orderService = orderService;
            _collisionDetector = collisionDetector;
        }
        
        public void AddMapping(SlicableObjectSpawnerData spawnerData)
        {
            SlicableObjectView slicableObjectView = _objectPool.InactiveItems.GetInactiveObject();
            Transform slicableViewTransform       = slicableObjectView.transform;
            
            _collisionDetector.AddCollider(slicableObjectView.Collider2D, slicableObjectView);
            
            _orderService.UpdateOrderInLayer(slicableObjectView.MainSprite);
            _orderService.UpdateOrderInLayer(slicableObjectView.ShadowSprite);

            UpdateViewSprites(slicableObjectView);
            ChangePositionAndActivate(spawnerData, slicableObjectView);

            float angleInRadians = GetDirectionAngleInRadians(spawnerData);
            float velocityX      = spawnerData.XVelocity.GetRandomValue();
            float velocityY      = spawnerData.YVelocity.GetRandomValue();
            
            velocityY = CalculateMaxSpeedY(velocityY, angleInRadians, slicableViewTransform.transform.position.y);

            IModelAnimation modelAnimation = GetModelAnimation(slicableViewTransform, slicableObjectView.ShadowSprite.transform, 0f);
            SlicableModel slicableModel    = new(slicableViewTransform, velocityX, velocityY, angleInRadians, modelAnimation);
            
            _slicableMovementService.AddMapping(slicableModel, slicableViewTransform);
        }

        private IModelAnimation GetModelAnimation(Transform slicableViewTransform, Transform shadowSpriteTransform, float angle)
        {
            return Random.Range(0, 3) switch
            {
                0 => new SimpleRotateAnimation(slicableViewTransform, angle),
                1 => new ScaleAnimation(slicableViewTransform, shadowSpriteTransform),
                2 => new MixedAnimation(slicableViewTransform, shadowSpriteTransform, angle),
                
                _ => throw new ArgumentException()
            };
        }
        
        private float CalculateMaxSpeedY(float velocity, float radians, float spawnPositionY)
        {
            float maxHeight = _gameScreenManager.GetOrthographicSize() + Mathf.Abs(spawnPositionY);
            float maxVelocity = Mathf.Sqrt(maxHeight * -2f * World.Gravity * Mathf.Sin(radians) * Mathf.Sin(radians));

            if (velocity > maxVelocity)
            {
                return maxVelocity - 1f;
            }
            
            return velocity;
        }

        private float GetDirectionAngleInRadians(SlicableObjectSpawnerData spawnerData)
        {
            if (spawnerData.FirstOffset > spawnerData.SecondOffset)
            {
                (spawnerData.FirstOffset, spawnerData.SecondOffset) = (spawnerData.SecondOffset, spawnerData.FirstOffset);
            }
            
            float randomAngle = Random.Range(spawnerData.FirstOffset, spawnerData.SecondOffset);

            return (spawnerData.MainDirectionOffset + 90f + randomAngle).ConvertToRadians();
        }

        private void ChangePositionAndActivate(SlicableObjectSpawnerData spawnerData, SlicableObjectView slicableObjectView)
        {
            Vector3 spawnPosition = _gameScreenManager.GetRandomPositionBetweenTwoPercents(spawnerData);

            slicableObjectView.transform.position = spawnPosition;
            slicableObjectView.gameObject.SetActive(true);
        }

        private void UpdateViewSprites(SlicableObjectView slicableObjectView)
        {
            Sprite sprite = _slicableVisualContainer.GetRandomSprite(SlicableObjectType.Simple);

            slicableObjectView.MainSprite.sprite = sprite;
            slicableObjectView.ShadowSprite.sprite = sprite;
        }
    }
}