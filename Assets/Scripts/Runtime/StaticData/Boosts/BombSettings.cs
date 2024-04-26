using UnityEngine;

namespace Runtime.StaticData.Boosts
{
    [CreateAssetMenu(menuName = "Settings/Game/Bomb", fileName = "BombStaticData")]
    public sealed class BombSettings : ScriptableObject
    {
        public float Radius;
        public float MaxForce;
    }
}