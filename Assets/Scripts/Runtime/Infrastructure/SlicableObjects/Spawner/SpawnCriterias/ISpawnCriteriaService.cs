using System.Collections.Generic;
using Unity.VisualScripting;

namespace Runtime.Infrastructure.SlicableObjects.Spawner.SpawnCriterias
{
    public interface ISpawnCriteriaService
    {
        List<SliceableObjectSpawnerData> Resolve(List<SliceableObjectSpawnerData> list);
    }
}