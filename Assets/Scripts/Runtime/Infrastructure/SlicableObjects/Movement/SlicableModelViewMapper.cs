using System;
using System.Linq;
using Runtime.Constants;
using Runtime.Extensions;
using Runtime.Infrastructure.SlicableObjects.Movement.Animation;
using Runtime.Infrastructure.SlicableObjects.Spawner;
using Unity.Mathematics;
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

        public SlicableModelViewMapper(
            SlicableObjectView.Pool objectPool,
            SlicableVisualContainer slicableVisualContainer,
            GameScreenManager gameScreenManager,
            SlicableMovementService slicableMovementService)
        {
            _objectPool = objectPool;
            _slicableVisualContainer = slicableVisualContainer;
            _gameScreenManager = gameScreenManager;
            _slicableMovementService = slicableMovementService;
        }
        
        public void AddMapping(SlicableObjectSpawnerData spawnerData)
        {
            SlicableObjectView slicableObjectView = _objectPool.InactiveItems.First(x => !x.gameObject.activeInHierarchy);
            Transform slicableViewTransform       = slicableObjectView.transform;

            UpdateViewSprites(slicableObjectView);
            ChangePositionAndActivate(spawnerData, slicableObjectView);

            Vector2 direction = GetDirection(spawnerData, slicableObjectView.transform.position).normalized;

            float speedX = Random.Range(spawnerData.SpeedXMin, spawnerData.SpeedXMax);
            float speedY = Random.Range(spawnerData.SpeedYMin, spawnerData.SpeedYMax);

            RecalculateSpeeds(spawnerData, direction, slicableViewTransform, ref speedX, ref speedY);
            
            IModelAnimation modelAnimation = GetModelAnimation(slicableViewTransform, slicableObjectView.ShadowSprite.transform, 0f);
            
            
            SlicableModel slicableModel = new(slicableViewTransform, speedX, speedY, direction, modelAnimation, spawnerData.SideType);
            
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

        private void RecalculateSpeeds(SlicableObjectSpawnerData spawnerData, Vector2 direction, Transform slicableViewTransform, ref float speedX, ref float speedY)
        {
            speedY = CalculateSpeedY(speedY, direction, slicableViewTransform.position, spawnerData.SideType);

            if (spawnerData.SideType is SideType.Left or SideType.Right)
            {
                if (speedX < 2f)
                {
                    speedX = 2f;
                }
            }
        }

        //TODO Убрать цикл. Сделать 1ой формулой
        private float CalculateSpeedY(float speedY, Vector2 direction, Vector3 position, SideType sideType)
        { 
            float angle = direction.GetAngleBetweenVectorAndHorizontalAxis();

            angle += sideType.GetAngle();
            
            float radians = angle.ConvertToRadians();
            
            float constantValue = Mathf.Sin(radians) * Mathf.Sin(radians) / 2f / World.Gravity * -1;
            
            float maxHeight = speedY * speedY * constantValue;

            while (maxHeight >= Mathf.Abs(_gameScreenManager.GetOrthographicSize() - position.y) - 0.5f)
            {
                speedY -= 0.05f;
                maxHeight = speedY * speedY * constantValue;
            }

            return speedY;
        }

        private Vector2 GetDirection(SlicableObjectSpawnerData spawnerData, Vector2 firstPosition)
        {
            float quaternionAngle = Quaternion.Angle(Quaternion.Euler(0f, 0f, spawnerData.FirstOffset), Quaternion.Euler(0f, 0f, spawnerData.SecondOffset));

            float randomAngle = Random.Range(-quaternionAngle, quaternionAngle);

            Vector2 directionPoint = _gameScreenManager.GetRotatableVectorPoint(spawnerData.MainDirectionOffset + randomAngle);

            return directionPoint - firstPosition;
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