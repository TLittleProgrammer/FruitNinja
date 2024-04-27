using UnityEngine;

namespace Runtime.StaticData.Boosts
{
    [CreateAssetMenu(menuName = "Settings/Game/Avosjka", fileName = "AvosjkaStaticData")]
    public sealed class AvosjkaSettings : ScriptableObject
    {
        [Range(2, 5)]
        public int PackSize;
        [Min(0.2f)]
        public float Invulnerability;
    }
}