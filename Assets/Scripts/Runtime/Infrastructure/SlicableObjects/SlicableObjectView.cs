using UnityEngine;
using Zenject;
using Vector3 = UnityEngine.Vector3;

namespace Runtime.Infrastructure.SlicableObjects
{
    public class SlicableObjectView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _mainSprite;
        [SerializeField] private SpriteRenderer _shadowSprite;
        [SerializeField] private CircleCollider2D _circleCollider2D;

        public SpriteRenderer MainSprite   => _mainSprite;
        public SpriteRenderer ShadowSprite => _shadowSprite;
        public CircleCollider2D CircleCollider => _circleCollider2D;
        
        private void Reset(Vector3 startPosition)
        {
            transform.position = startPosition;
        }

        public class Pool : MonoMemoryPool<Vector3, SlicableObjectView>
        {
            protected override void Reinitialize(Vector3 position, SlicableObjectView slicableObjectView)
            {
                slicableObjectView.Reset(position);
            }
        }
    }
}