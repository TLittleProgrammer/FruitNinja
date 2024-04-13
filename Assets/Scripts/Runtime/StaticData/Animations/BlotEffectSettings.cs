using UnityEngine;

namespace Runtime.StaticData.Animations
{
    [CreateAssetMenu(menuName = "Settings/Game/Blot Effect", fileName = "BlotEffectSettings")]
    public class BlotEffectSettings : ScriptableObject
    {
        public float Duration;
        
        public float MinDelay;
        public float MaxDelay;
        
        public float MinScale;
        public float MaxScale;

        private void OnValidate()
        {
            if (MinDelay < 0f)
            {
                MinDelay = 0f;
            }

            if (MaxDelay < MinDelay)
            {
                MaxDelay = MinDelay;
            }

            if (MinScale < 1f)
            {
                MinScale = 1f;
            }

            if (MaxScale < MinScale)
            {
                MaxScale = MinDelay;
            }
        }
    }
}