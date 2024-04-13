using System.Linq;
using Runtime.Constants;
using Runtime.Extensions;
using Runtime.Infrastructure.SlicableObjects.Spawner;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Runtime.Infrastructure.SlicableObjects.Movement
{
    public class SlicableModelViewMapper
    {
        private readonly SlicableObjectView.Pool _objectPool;
        private readonly SlicableSpriteContainer _slicableSpriteContainer;
        private readonly GameScreenManager _gameScreenManager;
        private readonly SlicableMovementService _slicableMovementService;

        public SlicableModelViewMapper(
            SlicableObjectView.Pool objectPool,
            SlicableSpriteContainer slicableSpriteContainer,
            GameScreenManager gameScreenManager,
            SlicableMovementService slicableMovementService)
        {
            _objectPool = objectPool;
            _slicableSpriteContainer = slicableSpriteContainer;
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

            SlicableModel slicableModel = new(speedX, speedY, direction, slicableViewTransform.position, spawnerData.SideType);
            
            _slicableMovementService.AddMapping(slicableModel, slicableViewTransform);
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

        private float CalculateSpeedY(float speedY, Vector2 direction, Vector3 position, SideType sideType)
        { 
            float angle = direction.GetAngleBetweenVectorAndHorizontalAxis();

            angle += sideType.GetAngle();
            
            float radians = angle.ConvertToRadians();
            
            float constantValue = Mathf.Sin(radians) * Mathf.Sin(radians) / 2f / World.Gravity * -1;
            
            float maxHeight = speedY * speedY * constantValue;

            while (maxHeight >= Mathf.Abs(_gameScreenManager.GetOrthographicSize() - position.y) - 0.5f)
            {
                speedY -= 0.25f;
                maxHeight = speedY * speedY * constantValue;
            }

            return speedY;
        }

        private Vector2 GetDirection(SlicableObjectSpawnerData spawnerData, Vector2 firstPosition)
        {
            //TODO использовать OnValidate, чтобы убрать эту проверку
            float randomAngle =
                spawnerData.FirstOffset >= spawnerData.SecondOffset ?
                    Random.Range(spawnerData.SecondOffset, spawnerData.FirstOffset) :
                    Random.Range(spawnerData.FirstOffset, spawnerData.SecondOffset);

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
            Sprite sprite = _slicableSpriteContainer.GetRandomSprite(SlicableObjectType.Simple);

            slicableObjectView.MainSprite.sprite = sprite;
            slicableObjectView.ShadowSprite.sprite = sprite;
        }
    }
}