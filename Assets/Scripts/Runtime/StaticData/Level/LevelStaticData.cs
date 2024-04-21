using System.Collections.Generic;
using Runtime.Infrastructure.Combo;
using Runtime.Infrastructure.SlicableObjects.Spawner;
using UnityEngine;

namespace Runtime.StaticData.Level
{
    [CreateAssetMenu(menuName = "Settings/Game/Level Static Data", fileName = "LevelStaticData")]
    public sealed class LevelStaticData : ScriptableObject
    {
        public List<SlicableObjectSpawnerData> SlicableObjectSpawnerDataList;
        public float BeginPackOffset;
        public float EndPackOffset;
        public int HealthCount;
        public ComboData ComboData;
    }
}