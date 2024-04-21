using UnityEngine;
using Zenject;
using Vector3 = UnityEngine.Vector3;

namespace Runtime.Infrastructure.SlicableObjects
{
    public class SlicableObjectView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _mainSprite;
        [SerializeField] private SpriteRenderer _shadowSprite;
        [SerializeField] private Collider2D _collider2D;

        public SlicableObjectType SlicableObjectType;

        public SpriteRenderer MainSprite   => _mainSprite;
        public SpriteRenderer ShadowSprite => _shadowSprite;
        public Collider2D Collider2D => _collider2D;

        public class Pool : MonoMemoryPool<Vector3, SlicableObjectView>
        {
        }
    }
}