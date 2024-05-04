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
        [SerializeField] private RectTransform _textTransform;
        [SerializeField] private Transform _scoreTransform;

        private RectTransform _rectTransform;
        private float _animationTime;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            Sequence = 
                DOTween.Sequence()
                    .Append(_scoreTransform.DOScale(Vector3.one, 0.2f).SetEase(Ease.InQuad))
                    .Append(_textTransform.DOAnchorPosY(_textTransform.anchoredPosition.y + 150f, 1f))
                    .Append(_scoreTransform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InQuint))
                    .OnComplete(() =>
                    {
                        gameObject.SetActive(false);
                    })
                    .SetAutoKill(false);
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
            
            Sequence.Restart();
        }

        public class Pool : MonoMemoryPool<ScoreEffect>
        {
        }
    }
}