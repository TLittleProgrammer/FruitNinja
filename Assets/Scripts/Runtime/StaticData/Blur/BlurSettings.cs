using UnityEngine;

namespace Runtime.StaticData.Blur
{
    [CreateAssetMenu(menuName = "Settings/Game/Blur", fileName = "BlurStaticData")]
    public sealed class BlurSettings : ScriptableObject
    {
        public float Target;
        public float Duration;
        public float InitialSize;
    }
}