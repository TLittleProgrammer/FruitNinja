using DG.Tweening;
using UnityEngine;
using Zenject;

namespace Runtime.Infrastructure.Effects
{
    [RequireComponent(typeof(RectTransform))]
    public sealed class HeartSplash : MonoBehaviour
    {
        private RectTransform _rectTransform;
        private Sequence _sequence;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        public void Play(Vector2 position)
        {
            _rectTransform.position = position;
            
            
            gameObject.SetActive(true);
            transform.localScale = Vector3.zero;
            
            _sequence?.Kill();
            _sequence = DOTween.Sequence();

            _sequence.Append(transform.DOScale(Vector3.one, 0.5f));
            _sequence.Append(transform.DORotateQuaternion(Quaternion.Euler(0f, 0f, 25f), 0.25f).SetLoops(5, LoopType.Yoyo));
            _sequence.Append(transform.DOScale(Vector3.zero, 0.5f));
            _sequence.OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
        }
        
        public class Pool : MonoMemoryPool<HeartSplash>
        {
        }
    }
}