using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Runtime.Infrastructure.Slicer.SliceServices.HealthFlying
{
    [RequireComponent(typeof(RectTransform))]
    public sealed class FlyingHealthView : MonoBehaviour
    {
        public Image Image;
        public RectTransform ImageRect;
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