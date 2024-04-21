using UnityEngine;
using Zenject;

namespace Runtime.Infrastructure.SlicableObjects
{
    public class SliceableObjectDummy : MonoBehaviour
    {
        public SlicableObjectView SlicableObjectView;

        public void ChangeSprite(Sprite sprite)
        {
            SlicableObjectView.MainSprite.sprite = sprite;
            SlicableObjectView.ShadowSprite.sprite = sprite;
        }

        public class Pool : MonoMemoryPool<Vector3, SliceableObjectDummy>
        {
        }
    }
}