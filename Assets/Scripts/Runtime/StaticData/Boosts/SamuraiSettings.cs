using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

namespace Runtime.StaticData.Boosts
{
    [CreateAssetMenu(menuName = "Settings/Game/Samurai", fileName = "SamuraiStaticData")]
    public sealed class SamuraiSettings : ScriptableObject
    {
        public float TimeDivide;
        public float SpawnOffsetDivide;
        public int Duration;
    }
}