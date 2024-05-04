using System.Collections.Generic;
using System.Linq;
using Runtime.Extensions;
using Runtime.Infrastructure.Game;
using Runtime.Infrastructure.StateMachine;

namespace Runtime.Infrastructure.SlicableObjects.Spawner.SpawnCriterias
{
    public sealed class SpawnCriteriaService : ISpawnCriteriaService
    {
        private readonly ISlicableObjectCounterOnMap _slicableObjectCounterOnMap;
        private readonly GameParameters _gameParameters;
        private readonly IGameStateMachine _gameStateMachine;

        public SpawnCriteriaService(ISlicableObjectCounterOnMap slicableObjectCounterOnMap, GameParameters gameParameters)
        {
            _slicableObjectCounterOnMap = slicableObjectCounterOnMap;
            _gameParameters = gameParameters;
        }
        
        public List<SliceableObjectSpawnerData> Resolve(List<SliceableObjectSpawnerData> list)
        {
            List<SliceableObjectSpawnerData> newList = list.CreateClone();

            if (_slicableObjectCounterOnMap.GetCountByType(SlicableObjectType.Brick) >= 1)
            {
                newList = newList.Where(x => x.SlicableObjectType is not SlicableObjectType.Brick).ToList();
            }

            if (_gameParameters.CurrentHealthIsMax)
            {
                newList = newList.Where(x => x.SlicableObjectType is not SlicableObjectType.Health).ToList();
            }

            return newList;
        }
    }
}