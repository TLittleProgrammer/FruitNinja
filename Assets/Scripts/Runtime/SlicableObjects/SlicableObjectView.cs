using UnityEngine;
using Zenject;

namespace Runtime.SlicableObjects
{
    public class SlicableObjectView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _mainSprite;
        [SerializeField] private SpriteRenderer _shadowSprite;

        public SpriteRenderer MainSprite   => _mainSprite;
        public SpriteRenderer ShadowSprite => _shadowSprite;
        
        private void Reset(Vector3 startPosition)
        {
            transform.position = startPosition;
        }

        public class Pool : MonoMemoryPool<Vector3, SlicableObjectView>
        {
            protected override void Reinitialize(Vector3 velocity, SlicableObjectView slicableObjectView)
            {
                slicableObjectView.Reset(velocity);
            }
        }
    }
}