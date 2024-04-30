using UnityEngine;

namespace Runtime.StaticData.Boosts
{
    [CreateAssetMenu(menuName = "Settings/Game/Ice", fileName = "IceStaticData")]
    public sealed class IceSettings : ScriptableObject
    {
        [Range(0, 1f)]
        public float TargetTimeScale;
        public float Duration;
        public float DurationToReturnNormalTimeScale;
    }
}