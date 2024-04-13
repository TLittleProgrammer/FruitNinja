using DG.Tweening;
using UnityEngine;

namespace Runtime.StaticData.Animations
{
    [CreateAssetMenu(menuName = "Game/Settings/Button Animation", fileName = "ButtonAnimationSettings")]
    public sealed class ButtonAnimationSettings : ScriptableObject
    {
        [Min(0.1f)]
        public float Duration;

        public Vector3 TargetScale;

        public Ease Ease;

        public Sequence Tween;
    }
}