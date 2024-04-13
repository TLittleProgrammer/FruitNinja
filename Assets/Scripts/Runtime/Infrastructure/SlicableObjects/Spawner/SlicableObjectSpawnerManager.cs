using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Runtime.Infrastructure.SlicableObjects.Movement;
using Runtime.StaticData.Level;
using UnityEngine;
using ITickable = Zenject.ITickable;

namespace Runtime.Infrastructure.SlicableObjects.Spawner
{
    public class SlicableObjectSpawnerManager : ITickable
    {
        private readonly SlicableModelViewMapper _slicableModelViewMapper;
        private readonly List<SlicableObjectSpawnerData> _spawnersData;

        private const float SpawnTime = 2.5f;
        
        private bool _canCalculateTime = true;
        private float _currentTime;
        private int _allWeightLine;
        
        public SlicableObjectSpawnerManager(LevelStaticData levelStaticData, SlicableModelViewMapper slicableModelViewMapper)
        {
            _slicableModelViewMapper = slicableModelViewMapper;
            _spawnersData = levelStaticData.SlicableObjectSpawnerDataList;
            
            CalculateWeightLine(levelStaticData);
        }

        public async void Tick()
        {
            if (_canCalculateTime is false)
                return;

            await CalculateTime();
        }

        private async UniTask CalculateTime()
        {
            _currentTime += Time.deltaTime;

            if (_currentTime >= SpawnTime)
            {
                int spawnerDataIndex = ChooseSpawnerDataIndex();

                _currentTime = 0f;
                _canCalculateTime = false;

                //TODO попахивает code smells. На будущее: убрать магические числа, PackSize указывать между двумя числами
                for (int i = 0; i < _spawnersData[spawnerDataIndex].PackSize; i++)
                {
                    _slicableModelViewMapper.AddMapping(_spawnersData[spawnerDataIndex]);
                    
                    await UniTask.Delay(Random.Range(200, 550));
                }

                _canCalculateTime = true;
            }
        }

        private int ChooseSpawnerDataIndex()
        {
            int randomedWeight = Random.Range(0, _allWeightLine);
            int currentValue = 0;

            for (int i = 0; i < _spawnersData.Count; i++)
            {
                currentValue += _spawnersData[i].Weight;
                
                if (currentValue >= randomedWeight)
                {
                    return i;
                }
            }

            return 0;
        }

        private void CalculateWeightLine(LevelStaticData levelStaticData)
        {
            foreach (SlicableObjectSpawnerData spawnerData in levelStaticData.SlicableObjectSpawnerDataList)
            {
                _allWeightLine += spawnerData.Weight;
            }
        }
    }
}