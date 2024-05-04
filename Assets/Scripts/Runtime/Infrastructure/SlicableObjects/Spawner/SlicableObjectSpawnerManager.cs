using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Runtime.Infrastructure.SlicableObjects.Movement;
using Runtime.Infrastructure.SlicableObjects.Spawner.SpawnCriterias;
using Runtime.Infrastructure.StateMachine;
using Runtime.Infrastructure.StateMachine.States;
using Runtime.StaticData.Level;
using UnityEngine;
using IInitializable = Zenject.IInitializable;
using ITickable = Zenject.ITickable;
using Random = UnityEngine.Random;

namespace Runtime.Infrastructure.SlicableObjects.Spawner
{
    public class SlicableObjectSpawnerManager : ITickable, IInitializable
    {
        private readonly SlicableModelViewMapper _slicableModelViewMapper;
        private readonly ISpawnCriteriaService _spawnCriteriaService;
        private readonly IGameStateMachine _gameStateMachine;
        private readonly ISlicableObjectCounterOnMap _slicableObjectCounterOnMap;
        private readonly List<SlicableObjectSpawnerData> _spawnersData;
        private readonly List<int> _spawnerPackResize;
        private readonly float _targetSpawnTime;

        private int _allWeightLine;
        private bool _canCalculateTime = true;
        private bool _stop = false;
        private float _spawnTime;
        private float _currentTime;
        private float _spawnOffsetDivider = 1;

        public SlicableObjectSpawnerManager(
            LevelStaticData levelStaticData,
            SlicableModelViewMapper slicableModelViewMapper,
            ISpawnCriteriaService spawnCriteriaService,
            IGameStateMachine gameStateMachine)
        {
            _slicableModelViewMapper = slicableModelViewMapper;
            _spawnCriteriaService = spawnCriteriaService;
            _gameStateMachine = gameStateMachine;
            _spawnersData = levelStaticData.SlicableObjectSpawnerDataList;
            _spawnTime = levelStaticData.BeginPackOffset;
            _targetSpawnTime = levelStaticData.EndPackOffset;
            _spawnerPackResize = new();
        }

        public void Initialize()
        {
            InitializeRepackSize();
            CalculateWeightLineForSpawners();
        }

        public async void Tick()
        {
            if (_canCalculateTime is false || _stop)
                return;

            await CalculateTime();
        }

        public void SetStop(bool value)
        {
            _stop = value;
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public async void UpdateSpawnSettings(float duration, float spawnOffsetDivider, float spawnDivider)
        {
            float originalSpawnTime = _spawnTime;
            _spawnOffsetDivider = spawnOffsetDivider;
            _spawnTime /= spawnDivider;

            await UniTask.Delay((int)(duration * 1000));

            _spawnTime = originalSpawnTime;
            _spawnOffsetDivider = 1;

            _stop = true;

            await UniTask.Delay(2000);

            _stop = false;
        }

        private async UniTask CalculateTime()
        {
            _currentTime += Time.deltaTime;

            if (_currentTime >= _spawnTime)
            {
                await Spawn();
            }
        }

        private async UniTask Spawn()
        {
            int spawnerDataIndex = ChooseSpawnerDataIndex();
            SlicableObjectSpawnerData spawnerData = _spawnersData[spawnerDataIndex];

            _currentTime = 0f;
            _canCalculateTime = false;

            int packSize = _spawnerPackResize[spawnerDataIndex] + spawnerData.PackSize;

            Array type = ChooseSlicableObjectType(spawnerData.SlicableObjectSpawnerDatas, packSize).ToArray();

            foreach (SlicableObjectType objectType in type)
            {
                SlicableObjectType targetType = objectType;
                
                if (_stop)
                {
                    _canCalculateTime = true;
                    return;
                }

                if (_gameStateMachine.CurrentState is SamuraiState)
                {
                    targetType = SlicableObjectType.Simple;
                    foreach (SlicableObjectSpawnerData spawner in _spawnersData)
                    {
                        _slicableModelViewMapper.AddMapping(spawner, targetType);
                        _slicableModelViewMapper.AddMapping(spawner, targetType);
                        _slicableModelViewMapper.AddMapping(spawner, targetType);
                    }

                    break;
                }
                
                _slicableModelViewMapper.AddMapping(_spawnersData[spawnerDataIndex], targetType);

                int delay = (int)(spawnerData.SpawnOffset.GetRandomValue() * 1000 / _spawnOffsetDivider);
                await UniTask.Delay(delay);
            }

            if (_spawnerPackResize[spawnerDataIndex] < 2)
            {
                _spawnerPackResize[spawnerDataIndex]++;
            }

            _canCalculateTime = true;

            if (_spawnTime - 0.1f >= _targetSpawnTime)
            {
                _spawnTime -= 0.1f;
            }
        }

        private List<SlicableObjectType> ChooseSlicableObjectType(List<SliceableObjectSpawnerData> slicableObjectSpawnerDatas, int packSize)
        {
            List<SliceableObjectSpawnerData> spawnerDatas = _spawnCriteriaService.Resolve(slicableObjectSpawnerDatas);
            float weightLine = spawnerDatas.Sum(x => x.Weight);

            List<SlicableObjectType> result = new();
            
            for (int i = 0; i < packSize; i++)
            {
                if (_stop)
                {
                    _canCalculateTime = true;
                    break;
                }
                
                result.Add(CalculateSpawnType(spawnerDatas, weightLine));
            }

            int maxBombSize = packSize / 2;
            int bombCounter = result.Count(x => x is SlicableObjectType.Bomb);
            if (bombCounter > maxBombSize)
            {
                int needToChange = bombCounter - maxBombSize;
                for (int i = 0; needToChange > 0; i++)
                {
                    if (result[i] == SlicableObjectType.Bomb)
                    {
                        result[i] = SlicableObjectType.Simple;
                        needToChange--;
                    }
                }
            }

            return result;
        }

        private SlicableObjectType CalculateSpawnType(List<SliceableObjectSpawnerData> spawnerDatas, float weightLine)
        {
            float currentWeight = 0f;
            float targetWeight  = Random.Range(0f, weightLine);

            for (int i = 0; i < spawnerDatas.Count; i++)
            {
                currentWeight += spawnerDatas[i].Weight;

                if (targetWeight <= currentWeight)
                {
                    return spawnerDatas[i].SlicableObjectType;
                }
            }

            return SlicableObjectType.Simple;
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

        private void CalculateWeightLineForSpawners()
        {
            foreach (SlicableObjectSpawnerData spawnerData in _spawnersData)
            {
                _allWeightLine += spawnerData.Weight;
            }
        }
    }
}