using Runtime.Infrastructure.DOTweenAnimationServices.Score;
using Runtime.Infrastructure.Game;
using Runtime.Infrastructure.UserData;
using TMPro;
using UnityEngine;
using Zenject;

namespace Runtime.UI.Game
{
    public class GameScoreText : MonoBehaviour
    {
        [SerializeField] private TMP_Text _currentScoreText;
        [SerializeField] private TMP_Text _bestScoreText;

        private int _currentScoreValue;
        private int _currentBestScoreValue;
        private IScoreAnimationService _scoreAnimationService;

        [Inject]
        private void Construct(GameParameters gameParameters, UserData userData, IScoreAnimationService scoreAnimationService)
        {
            _scoreAnimationService = scoreAnimationService;
            gameParameters.ScoreChanged += OnScoreChanged;
            userData.BestScoreChanged += OnBestScoreChanged;

            _currentScoreValue = gameParameters.CurrentScore;
            _currentBestScoreValue = userData.bestScore;

            _currentScoreText.text = _currentScoreValue.ToString();
            _bestScoreText.text = _currentBestScoreValue.ToString();
        }

        private void OnScoreChanged(int newScore)
        {
            _scoreAnimationService.Animate(_currentScoreText, _currentScoreValue, newScore);
            _currentScoreValue = newScore;
        }

        private void OnBestScoreChanged(int newBestScore)
        {
            _scoreAnimationService.Animate(_bestScoreText, _currentBestScoreValue, newBestScore);
            _currentBestScoreValue = newBestScore;
        }
    }
}