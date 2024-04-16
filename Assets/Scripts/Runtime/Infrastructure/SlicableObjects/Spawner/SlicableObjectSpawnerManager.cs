using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Runtime.Infrastructure.SlicableObjects.Movement;
using Runtime.StaticData.Level;
using Unity.VisualScripting;
using UnityEngine;
using IInitializable = Zenject.IInitializable;
using ITickable = Zenject.ITickable;

namespace Runtime.Infrastructure.SlicableObjects.Spawner
{
    public class SlicableObjectSpawnerManager : ITickable, IInitializable
    {
        private readonly SlicableModelViewMapper _slicableModelViewMapper;
        private readonly List<SlicableObjectSpawnerData> _spawnersData;
        private readonly List<int> _spawnerPackResize;
        private readonly float _targetSpawnTime;

        private int _allWeightLine;
        private bool _canCalculateTime = true;
        private float _spawnTime;
        private float _currentTime;

        public SlicableObjectSpawnerManager(LevelStaticData levelStaticData, SlicableModelViewMapper slicableModelViewMapper)
        {
            _slicableModelViewMapper = slicableModelViewMapper;
            _spawnersData = levelStaticData.SlicableObjectSpawnerDataList;
            _spawnTime = levelStaticData.BeginPackOffset;
            _targetSpawnTime = levelStaticData.EndPackOffset;
            _spawnerPackResize = new();
        }

        public void Initialize()
        {
            InitializeRepackSize();
            CalculateWeightLine();
        }

        public async void Tick()
        {
            if (_canCalculateTime is false)
                return;

            await CalculateTime();
        }

        public void Continue()
        {
            _canCalculateTime = true;
        }

        public void Stop()
        {
            _canCalculateTime = false;
        }

        private async UniTask CalculateTime()
        {
            _currentTime += Time.deltaTime;

            if (_currentTime >= _spawnTime)
            {
                int spawnerDataIndex = ChooseSpawnerDataIndex();
                SlicableObjectSpawnerData spawnerData = _spawnersData[spawnerDataIndex];
                
                _currentTime = 0f;
                _canCalculateTime = false;

                int packSize = _spawnerPackResize[spawnerDataIndex] + spawnerData.PackSize;

                for (int i = 0; i < packSize; i++)
                {
                    _slicableModelViewMapper.AddMapping(_spawnersData[spawnerDataIndex]);

                    int delay = (int)(Random.Range(spawnerData.PackSpawnOffsetMin, spawnerData.PackSpawnOffsetMax) * 1000);
                    
                    await UniTask.Delay(delay);
                }
                
                if (_spawnerPackResize[spawnerDataIndex] < 5)
                {
                    _spawnerPackResize[spawnerDataIndex]++;
                }
                
                _canCalculateTime = true;
                
                if (_spawnTime - 0.1f >= _targetSpawnTime)
                {
                    _spawnTime -= 0.1f;
                }
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

        private void InitializeRepackSize()
        {
            for (int i = 0; i < _spawnersData.Count; i++)
            {
                _spawnerPackResize.Add(0);
            }
        }

        private void CalculateWeightLine()
        {
            foreach (SlicableObjectSpawnerData spawnerData in _spawnersData)
            {
                _allWeightLine += spawnerData.Weight;
            }
        }
    }
}