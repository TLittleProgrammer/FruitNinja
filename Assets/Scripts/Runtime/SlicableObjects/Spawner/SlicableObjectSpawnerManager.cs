using System.Collections.Generic;
using Runtime.StaticData.Level;
using UnityEngine;
using ITickable = Zenject.ITickable;

namespace Runtime.SlicableObjects.Spawner
{
    public class SlicableObjectSpawnerManager : ITickable
    {
        private readonly List<SlicableObjectSpawnerData> _levelStaticData;

        private const float SpawnTime = 2f;
        private float _currentTime = 0f;
        private int _allWeightLine = 0;
        
        public SlicableObjectSpawnerManager(LevelStaticData levelStaticData)
        {
            _levelStaticData = levelStaticData.SlicableObjectSpawnerDataList;
            
            FillList(levelStaticData);
        }

        public void Tick()
        {
            _currentTime += Time.deltaTime;

            if (_currentTime >= SpawnTime)
            {
                int spawnerDataIndex = ChooseSpawnerData();

                
                
                _currentTime = 0f;
            }
        }

        private int ChooseSpawnerData()
        {
            int randomedWeight = Random.Range(0, _allWeightLine);
            int currentValue = 0;

            for (int i = 0; i < _levelStaticData.Count; i++)
            {
                if (currentValue >= randomedWeight)
                {
                    return i;
                }

                currentValue += _levelStaticData[i].Weight;
            }

            return 0;
        }

        private void FillList(LevelStaticData levelStaticData)
        {
            foreach (SlicableObjectSpawnerData spawnerData in levelStaticData.SlicableObjectSpawnerDataList)
            {
                _allWeightLine += spawnerData.Weight;
            }
        }
    }
}