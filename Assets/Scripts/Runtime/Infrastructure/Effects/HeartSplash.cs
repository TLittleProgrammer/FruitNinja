using DG.Tweening;
using Runtime.Infrastructure.Containers;
using Runtime.Infrastructure.SlicableObjects;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Runtime.Infrastructure.Effects
{
    [RequireComponent(typeof(RectTransform))]
    public sealed class HeartSplash : AddictableFromScale
    {
        public Image Sprite;
        
        private RectTransform _rectTransform;
        private SpriteProviderContainer _spriteProvider;

        [Inject]
        private void Construct(SpriteProviderContainer spriteProvider)
        {
            _spriteProvider = spriteProvider;
        }
        
        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        public void Play(Vector2 position, SlicableObjectType slicableObjectType)
        {
            _rectTransform.position = position;
            Sprite.sprite = _spriteProvider.GetSpriteByType(slicableObjectType); 
            
            gameObject.SetActive(true);
            transform.localScale = Vector3.zero;
            
            Sequence?.Kill();
            Sequence = DOTween.Sequence()
                .Append(transform.DOScale(Vector3.one, 0.15f))
                .AppendInterval(0.5f)
                .Append(transform.DOScale(Vector3.zero, 0.15f));
            
            Sequence.OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
        }
        
        public class Pool : MonoMemoryPool<HeartSplash>
        {
        }
    }
}