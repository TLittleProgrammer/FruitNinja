using ObjectPool.Runtime.ObjectPool;
using Runtime.Infrastructure;
using UnityEngine;
using Zenject;

namespace Runtime.SlicableObjects.Spawner
{
    public class SlicableObjectSpawnerService : ITickable
    {
        private readonly SlicableObjectSpawnerData _slicableObjectSpawnerData;
        private readonly QueueObjectPool<SlicableObjectView> _objectPool;
        private readonly GameScreenPositionResolver _gameScreenPositionResolver;
        private readonly SlicableSpriteContainer _spriteContainer;

        private float _currentTime;
        
        public SlicableObjectSpawnerService(
            SlicableObjectSpawnerData slicableObjectSpawnerData,
            QueueObjectPool<SlicableObjectView> objectPool,
            GameScreenPositionResolver gameScreenPositionResolver,
            SlicableSpriteContainer spriteContainer)
        {
            _slicableObjectSpawnerData = slicableObjectSpawnerData;
            _objectPool = objectPool;
            _gameScreenPositionResolver = gameScreenPositionResolver;
            _spriteContainer = spriteContainer;
            _currentTime = 0f;
        }

        private float TargetTime => _slicableObjectSpawnerData.TimeOffset;

        public void Tick()
        {
            _currentTime += Time.deltaTime;

            if (_currentTime >= TargetTime)
            {
                GetView();
                
                _currentTime = 0f;
            }
        }

        private void GetView()
        {
            SlicableObjectView slicableObjectView = _objectPool.Get();
            Vector2 viewPosition = _gameScreenPositionResolver.GetRandomPositionBetweenTwoPercents(_slicableObjectSpawnerData.FirstSpawnPoint, _slicableObjectSpawnerData.SecondSpawnPoint, _slicableObjectSpawnerData.SideType);

            Sprite sprite = _spriteContainer.GetRandomSprite(SlicableObjectType.Simple);

            slicableObjectView.MainSprite.sprite = sprite;
            slicableObjectView.ShadowSprite.sprite = sprite;
            
            slicableObjectView.transform.position = viewPosition;
            slicableObjectView.gameObject.SetActive(true);
            
            
        }
    }
}