using System.Collections.Generic;

namespace Runtime.Infrastructure.SlicableObjects.Spawner.SpawnCriterias
{
    public interface ISpawnCriteriaService
    {
        List<SliceableObjectSpawnerData> Resolve(List<SliceableObjectSpawnerData> list);
    }
}