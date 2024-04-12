using System.Collections.Generic;
using ObjectPool.Runtime.ObjectPool;
using Runtime.Infrastructure;
using Runtime.SlicableObjects.Spawner;
using UnityEngine;
using Zenject;

namespace Runtime.SlicableObjects.Movement
{
    public class SlicableMovementService : ITickable
    {
        private readonly QueueObjectPool<SlicableObjectView> _objectPool;
        private readonly SlicableSpriteContainer _slicableSpriteContainer;
        private readonly GameScreenPositionResolver _gameScreenPositionResolver;

        private List<(SlicableModel, SlicableObjectView)> _slicableModels;

        public SlicableMovementService(QueueObjectPool<SlicableObjectView> objectPool, SlicableSpriteContainer slicableSpriteContainer, GameScreenPositionResolver gameScreenPositionResolver)
        {
            _objectPool = objectPool;
            _slicableSpriteContainer = slicableSpriteContainer;
            _gameScreenPositionResolver = gameScreenPositionResolver;
            _slicableModels = new();
        }
        
        public void Tick()
        {
            Debug.Log("FFFFF");
            foreach ((SlicableModel model, SlicableObjectView view) in _slicableModels)
            {
                Vector2 viewPosition = view.transform.position;

                viewPosition += new Vector2(model.SpeedX, model.SpeedY) * Time.deltaTime * model.Direction;

                model.SpeedY += Physics2D.gravity.y * Time.deltaTime;

                view.transform.position = viewPosition;
            }
        }

        public void AddSlicable(SlicableObjectSpawnerData spawnerData)
        {
            SlicableObjectView slicableObjectView = _objectPool.Get();

            Sprite sprite = _slicableSpriteContainer.GetRandomSprite(SlicableObjectType.Simple);

            slicableObjectView.MainSprite.sprite   = sprite;
            slicableObjectView.ShadowSprite.sprite = sprite;

            Vector3 spawnPosition = _gameScreenPositionResolver.GetRandomPositionBetweenTwoPercents(spawnerData);

            slicableObjectView.transform.position = spawnPosition;
            
            slicableObjectView.gameObject.SetActive(true);

            Vector2 middlePoint = _gameScreenPositionResolver.GetMiddlePoint(spawnerData);
            Vector2 directionToMiddleCameraPoint = (Vector2.zero - middlePoint).normalized;

            
            float randomAngle = 0f;
            
            if (spawnerData.FirstOffset >= spawnerData.SecondOffset)
            {
                randomAngle = Random.Range(spawnerData.SecondOffset, spawnerData.FirstOffset);
            }
            else
            {
                randomAngle = Random.Range(spawnerData.FirstOffset, spawnerData.SecondOffset);
            }

            Vector2 directionPoint = _gameScreenPositionResolver.GetRotatableVectorPoint(middlePoint, directionToMiddleCameraPoint, spawnerData.MainDirectionOffset + randomAngle).normalized;

            Vector2 direction = directionPoint - middlePoint;

            float speedX = Random.Range(spawnerData.SpeedXMin, spawnerData.SpeedXMax);
            float speedY = Random.Range(spawnerData.SpeedYMin, spawnerData.SpeedYMax);
            
            _slicableModels.Add((new(speedX, speedY, direction), slicableObjectView));
        }
    }
}