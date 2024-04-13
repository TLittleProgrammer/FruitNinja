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
        private readonly float _gravity;

        private List<(SlicableModel, SlicableObjectView)> _slicableModels;

        public SlicableMovementService(QueueObjectPool<SlicableObjectView> objectPool, SlicableSpriteContainer slicableSpriteContainer, GameScreenPositionResolver gameScreenPositionResolver)
        {
            _objectPool = objectPool;
            _slicableSpriteContainer = slicableSpriteContainer;
            _gameScreenPositionResolver = gameScreenPositionResolver;
            _slicableModels = new();
            _gravity = Physics.gravity.y / 2f;
        }
        
        public void Tick()
        {
            foreach ((SlicableModel model, SlicableObjectView view) in _slicableModels)
            {
                Vector2 viewPosition = view.transform.position;

                viewPosition += new Vector2(model.SpeedX, model.SpeedY) * Time.deltaTime * model.Direction;

                model.SpeedY += _gravity * Time.deltaTime ;

                view.transform.position = viewPosition;
            }
        }

        public void AddSlicable(SlicableObjectSpawnerData spawnerData)
        {
            SlicableObjectView slicableObjectView = _objectPool.Get();

            UpdateView(slicableObjectView);
            ChangePositionAndActivate(spawnerData, slicableObjectView);

            Vector2 middlePoint = _gameScreenPositionResolver.GetMiddlePoint(spawnerData);

            float randomAngle;
            
            if (spawnerData.FirstOffset >= spawnerData.SecondOffset)
            {
                randomAngle = Random.Range(spawnerData.SecondOffset, spawnerData.FirstOffset);
            }
            else
            {
                randomAngle = Random.Range(spawnerData.FirstOffset, spawnerData.SecondOffset);
            }

            Vector2 directionPoint = _gameScreenPositionResolver.GetRotatableVectorPoint(spawnerData.MainDirectionOffset + randomAngle);

            Vector2 direction = directionPoint - middlePoint;

            float speedX = Random.Range(spawnerData.SpeedXMin, spawnerData.SpeedXMax);
            float speedY = Random.Range(spawnerData.SpeedYMin, spawnerData.SpeedYMax);
            
            _slicableModels.Add((new(speedX, speedY, direction), slicableObjectView));
        }

        private void ChangePositionAndActivate(SlicableObjectSpawnerData spawnerData, SlicableObjectView slicableObjectView)
        {
            Vector3 spawnPosition = _gameScreenPositionResolver.GetRandomPositionBetweenTwoPercents(spawnerData);

            slicableObjectView.transform.position = spawnPosition;
            slicableObjectView.gameObject.SetActive(true);
        }

        private void UpdateView(SlicableObjectView slicableObjectView)
        {
            Sprite sprite = _slicableSpriteContainer.GetRandomSprite(SlicableObjectType.Simple);

            slicableObjectView.MainSprite.sprite = sprite;
            slicableObjectView.ShadowSprite.sprite = sprite;
        }
    }
}