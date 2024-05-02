using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Zenject;

namespace Runtime.Infrastructure.Effects
{
    public interface IScoreEffect
    {
        void PlayEffect(Vector3 screenPosition, int score);
    }
    
    [RequireComponent(typeof(RectTransform))]
    public sealed class ScoreEffect : PopupEffect, IScoreEffect
    {
        [SerializeField] private TMP_Text _scoreText;

        private RectTransform _rectTransform;
        private float _animationTime;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        public void PlayEffect(Vector3 screenPosition, int score)
        {
            _scoreText.text = score.ToString();
            _rectTransform.position = screenPosition;
            
            PlayAnimation();
        }

        private void PlayAnimation()
        {
            gameObject.SetActive(true);

            Sequence = DOTween.Sequence();
            Sequence.Append(transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.InQuad)).ToUniTask().Forget();
            Sequence.Append(_rectTransform.DOAnchorPosY(_rectTransform.anchoredPosition.y + 150f, 1f)).ToUniTask().Forget();
            Sequence.Append(transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InQuint)).ToUniTask().Forget();
            Sequence.OnComplete(() =>
            {
                gameObject.SetActive(false);
            }).ToUniTask().Forget();
        }

        public class Pool : MonoMemoryPool<ScoreEffect>
        {
        }
    }
}