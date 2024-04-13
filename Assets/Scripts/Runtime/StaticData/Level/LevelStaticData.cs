using System.Collections.Generic;
using Runtime.SlicableObjects.Spawner;
using UnityEngine;

namespace Runtime.StaticData.Level
{
    [CreateAssetMenu(menuName = "Settings/Game/Level Static Data", fileName = "LevelStaticData")]
    public sealed class LevelStaticData : ScriptableObject
    {
        public List<SlicableObjectSpawnerData> SlicableObjectSpawnerDataList;
    }
}