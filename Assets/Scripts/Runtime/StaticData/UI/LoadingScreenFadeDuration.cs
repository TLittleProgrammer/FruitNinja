using UnityEngine;

namespace Runtime.StaticData.UI
{
    [CreateAssetMenu(menuName = "Settings/Game/Loading Screen", fileName = "LoadingScreenSettings")]
    public sealed class LoadingScreenFadeDuration : ScriptableObject
    {
        public float Duration;
    }
}