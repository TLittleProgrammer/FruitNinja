using System.Linq;
using Runtime.Infrastructure.SlicableObjects.Spawner;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Runtime.Infrastructure.SlicableObjects.Movement
{
    public class SlicableModelViewMapper
    {
        private readonly SlicableObjectView.Pool _objectPool;
        private readonly SlicableSpriteContainer _slicableSpriteContainer;
        private readonly GameScreenPositionResolver _gameScreenPositionResolver;
        private readonly SlicableMovementService _slicableMovementService;

        public SlicableModelViewMapper(
            SlicableObjectView.Pool objectPool,
            SlicableSpriteContainer slicableSpriteContainer,
            GameScreenPositionResolver gameScreenPositionResolver,
            SlicableMovementService slicableMovementService)
        {
            _objectPool = objectPool;
            _slicableSpriteContainer = slicableSpriteContainer;
            _gameScreenPositionResolver = gameScreenPositionResolver;
            _slicableMovementService = slicableMovementService;
        }
        
        public void AddMapping(SlicableObjectSpawnerData spawnerData)
        {
            SlicableObjectView slicableObjectView = _objectPool.InactiveItems.First(x => !x.gameObject.activeInHierarchy);
            Transform slicableViewTransform       = slicableObjectView.transform;

            UpdateViewSprites(slicableObjectView);
            ChangePositionAndActivate(spawnerData, slicableObjectView);

            Vector2 direction = GetDirection(spawnerData);

            float speedX = Random.Range(spawnerData.SpeedXMin, spawnerData.SpeedXMax);
            float speedY = Random.Range(spawnerData.SpeedYMin, spawnerData.SpeedYMax);
            
            SlicableModel slicableModel = new(speedX, speedY, direction, slicableViewTransform.position);
            
            _slicableMovementService.AddMapping(slicableModel, slicableViewTransform);
        }

        private Vector2 GetDirection(SlicableObjectSpawnerData spawnerData)
        {
            Vector2 middlePoint = _gameScreenPositionResolver.GetMiddlePoint(spawnerData);

            float randomAngle =
                spawnerData.FirstOffset >= spawnerData.SecondOffset ?
                    Random.Range(spawnerData.SecondOffset, spawnerData.FirstOffset) :
                    Random.Range(spawnerData.FirstOffset, spawnerData.SecondOffset);

            Vector2 directionPoint =
                _gameScreenPositionResolver.GetRotatableVectorPoint(spawnerData.MainDirectionOffset + randomAngle);

            return directionPoint - middlePoint;
        }

        private void ChangePositionAndActivate(SlicableObjectSpawnerData spawnerData, SlicableObjectView slicableObjectView)
        {
            Vector3 spawnPosition = _gameScreenPositionResolver.GetRandomPositionBetweenTwoPercents(spawnerData);

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