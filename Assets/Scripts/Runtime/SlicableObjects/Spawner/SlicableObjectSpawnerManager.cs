using System.Collections.Generic;
using ObjectPool.Runtime.ObjectPool;
using Runtime.Infrastructure;
using Runtime.StaticData.Level;
using ITickable = Zenject.ITickable;

namespace Runtime.SlicableObjects.Spawner
{
    public class SlicableObjectSpawnerManager : ITickable
    {
        private readonly QueueObjectPool<SlicableObjectView> _objectPool;
        private readonly List<SlicableObjectSpawnerService> _slicableObjectSpawners;

        //TODO подумать над тем, чтобы создавать сервисы через DiContanier, чтобы не прокидывать в конструктор ссылки
        public SlicableObjectSpawnerManager(
            LevelStaticData levelStaticData,
            QueueObjectPool<SlicableObjectView> objectPool,
            GameScreenPositionResolver gameScreenPositionResolver,
            SlicableSpriteContainer spriteContainer)
        {
            _objectPool = objectPool;
            _slicableObjectSpawners = new();
            
            FillList(levelStaticData, gameScreenPositionResolver, spriteContainer);
        }

        public void Tick()
        {
            foreach (SlicableObjectSpawnerService service in _slicableObjectSpawners)
            {
                service.Tick();
            }
        }

        private void FillList(LevelStaticData levelStaticData, GameScreenPositionResolver gameScreenPositionResolver, SlicableSpriteContainer spriteContainer)
        {
            foreach (SlicableObjectSpawnerData spawnerData in levelStaticData.SlicableObjectSpawnerDataList)
            {
                _slicableObjectSpawners.Add(new(spawnerData, _objectPool, gameScreenPositionResolver, spriteContainer));
            }
        }
    }
}