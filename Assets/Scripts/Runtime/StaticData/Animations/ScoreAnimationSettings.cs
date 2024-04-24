using UnityEngine;

namespace Runtime.StaticData.Animations
{
    [CreateAssetMenu(menuName = "Game/Settings/Score Animation", fileName = "ScoreAnimationSettings")]
    public sealed class ScoreAnimationSettings : ScriptableObject
    {
        [Min(0.1f)]
        public float Duration;
    }
}