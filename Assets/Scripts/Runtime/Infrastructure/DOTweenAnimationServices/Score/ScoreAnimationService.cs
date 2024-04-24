using System.Collections.Generic;
using DG.Tweening;
using Runtime.StaticData.Animations;
using TMPro;

namespace Runtime.Infrastructure.DOTweenAnimationServices.Score
{
    public sealed class ScoreAnimationService : IScoreAnimationService
    {
        private readonly ScoreAnimationSettings _settings;
        private Dictionary<TMP_Text, Tweener> _scoreTweeners;

        public ScoreAnimationService(ScoreAnimationSettings settings)
        {
            _settings = settings;
            _scoreTweeners = new();
        }
        
        public void Animate(TMP_Text text, int from, int to)
        {
            if (!_scoreTweeners.ContainsKey(text))
            {
                _scoreTweeners.Add(
                    text,
                    DOVirtual.Int(from, to, _settings.Duration, score => SetScore(text, score))
                );
                
                return;
            }
            
            _scoreTweeners[text].Kill();
            _scoreTweeners[text] = DOVirtual.Int(from, to, _settings.Duration, score => SetScore(text, score));
        }

        public void Dispose()
        {
            foreach (KeyValuePair<TMP_Text,Tweener> pair in _scoreTweeners)
            {
                pair.Value.Kill();
            }
        }

        private void SetScore(TMP_Text text, int score)
        {
            text.text = score.ToString();
        }
    }
}