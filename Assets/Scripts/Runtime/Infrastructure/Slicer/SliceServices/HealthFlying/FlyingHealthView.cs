using UnityEngine;
using Zenject;

namespace Runtime.Infrastructure.Slicer.SliceServices.HealthFlying
{
    [RequireComponent(typeof(RectTransform))]
    public sealed class FlyingHealthView : MonoBehaviour
    {
        private RectTransform _rectTransform;

        public RectTransform RectTransform => _rectTransform;
        
        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        public class Pool : MonoMemoryPool<FlyingHealthView>
        {
        }
    }
}