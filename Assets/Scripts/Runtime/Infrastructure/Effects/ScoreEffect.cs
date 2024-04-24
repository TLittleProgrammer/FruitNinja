using System;
using System.Collections;
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
    public sealed class ScoreEffect : MonoBehaviour, IScoreEffect
    {
        [SerializeField] private TMP_Text _scoreText;
        [SerializeField] private Animator _animator;

        private readonly int ScoreFly = Animator.StringToHash("ScoreFly");
        
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
            
            gameObject.SetActive(true);
            StartCoroutine(PlayAnimation());
        }

        private IEnumerator PlayAnimation()
        {
            _animator.Play(ScoreFly);

            yield return new WaitForSeconds(1f);
            
            gameObject.SetActive(false);
        }

        public class Pool : MonoMemoryPool<ScoreEffect>
        {
        }
    }
}