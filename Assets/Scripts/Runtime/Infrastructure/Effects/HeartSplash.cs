using DG.Tweening;
using UnityEngine;
using Zenject;

namespace Runtime.Infrastructure.Effects
{
    [RequireComponent(typeof(RectTransform))]
    public sealed class HeartSplash : AddictableFromScale
    {
        private RectTransform _rectTransform;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        public void Play(Vector2 position)
        {
            _rectTransform.position = position;
            
            
            gameObject.SetActive(true);
            transform.localScale = Vector3.zero;
            
            Sequence?.Kill();
            Sequence = DOTween.Sequence();

            Sequence.Append(transform.DOScale(Vector3.one, 0.5f));
            Sequence.Append(transform.DORotateQuaternion(Quaternion.Euler(0f, 0f, 25f), 0.25f).SetLoops(5, LoopType.Yoyo));
            Sequence.Append(transform.DOScale(Vector3.zero, 0.5f));
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