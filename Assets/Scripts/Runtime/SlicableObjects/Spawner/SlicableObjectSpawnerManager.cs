using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Runtime.SlicableObjects.Movement;
using Runtime.StaticData.Level;
using UnityEngine;
using ITickable = Zenject.ITickable;

namespace Runtime.SlicableObjects.Spawner
{
    public class SlicableObjectSpawnerManager : ITickable
    {
        private readonly SlicableMovementService _slicableMovementService;
        private readonly List<SlicableObjectSpawnerData> _spawnersData;

        private const float SpawnTime = 4f;
        private bool _canCalculateTime = true;
        private float _currentTime = 0f;
        private int _allWeightLine = 0;
        
        public SlicableObjectSpawnerManager(LevelStaticData levelStaticData, SlicableMovementService slicableMovementService)
        {
            _slicableMovementService = slicableMovementService;
            _spawnersData = levelStaticData.SlicableObjectSpawnerDataList;
            
            CalculateWeightLine(levelStaticData);
        }

        public async void Tick()
        {
            if(_canCalculateTime)
                _currentTime += Time.deltaTime;

            if (_currentTime >= SpawnTime)
            {
                int spawnerDataIndex = ChooseSpawnerData();
                
                _currentTime = 0f;
                _canCalculateTime = false;
                
                for (int i = 0; i < _spawnersData[spawnerDataIndex].PackSize; i++)
                {
                    _slicableMovementService.AddSlicable(_spawnersData[spawnerDataIndex]);
                    await UniTask.Delay(Random.Range(200, 550));
                }

                _canCalculateTime = true;
            }
        }

        private async UniTask SpawnSlicableViews(int spawnerDataIndex)
        {
            
        }

        private int ChooseSpawnerData()
        {
            int randomedWeight = Random.Range(0, _allWeightLine);
            int currentValue = 0;

            for (int i = 0; i < _spawnersData.Count; i++)
            {
                if (currentValue >= randomedWeight)
                {
                    return i;
                }

                currentValue += _spawnersData[i].Weight;
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