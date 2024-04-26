using DG.Tweening;
using UnityEngine;

namespace Runtime.StaticData.UI
{
    [CreateAssetMenu(menuName = "Settings/Game/FlyingHealthView", fileName = "FlyingHealthSettings")]
    public class FlyingHealthViewStaticData : ScriptableObject
    {
        public float TimeDelayAfterAnimation;
        public float FlyDuration;
        public Ease Ease;

        public float DestroyDuration;
    }
}