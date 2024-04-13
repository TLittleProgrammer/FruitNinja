using UnityEngine;
using Zenject;

namespace Runtime.Infrastructure.SlicableObjects
{
    public class SliceableObjectDummy : MonoBehaviour
    {
        public SlicableObjectView SlicableObjectView;
        
        private void Reset(Vector3 startPosition)
        {
            transform.position = startPosition;
        }

        public class Pool : MonoMemoryPool<Vector3, SliceableObjectDummy>
        {
            protected override void Reinitialize(Vector3 position, SliceableObjectDummy dummy)
            {
                dummy.Reset(position);
            }
        }

        public void ChangeSprite(Sprite sprite)
        {
            SlicableObjectView.MainSprite.sprite = sprite;
            SlicableObjectView.ShadowSprite.sprite = sprite;
        }
    }
}