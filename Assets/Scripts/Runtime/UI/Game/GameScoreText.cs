using System;
using System.Collections;
using Cysharp.Threading.Tasks;
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

        private const float MaxTimeInMillisecondToChangeString = 0.3f;
        private const string BestScorePrefix = "Лучший: ";
        
        private int _currentScoreValue;
        private int _currentBestScoreValue;
        
        [Inject]
        private void Construct(GameParameters gameParameters, UserData userData)
        {
            gameParameters.ScoreChanged += OnScoreChanged;
            userData.BestScoreChanged += OnBestScoreChanged;

            _currentScoreValue = 0;
            _currentBestScoreValue = userData.bestScore;

            _currentScoreText.text = _currentScoreValue.ToString();
            _bestScoreText.text = BestScorePrefix + _currentBestScoreValue.ToString();
        }

        private async void OnScoreChanged(int newScore)
        {
            _currentScoreValue = await ChangeScoreText(_currentScoreText, String.Empty, _currentScoreValue, newScore);
        }

        private async void OnBestScoreChanged(int newBestScore)
        {
            _currentBestScoreValue = await ChangeScoreText(_bestScoreText, BestScorePrefix, _currentBestScoreValue, newBestScore);
        }

        private async UniTask<int> ChangeScoreText(TMP_Text scoreText, string prefix, int currentValue, int targetValue)
        {
            float timeOffset = MaxTimeInMillisecondToChangeString / (targetValue - currentValue);

            while (currentValue <= targetValue)
            {
                currentValue++;
                scoreText.text = prefix + currentValue.ToString();

                await UniTask.Delay((int)(timeOffset * 1000));
            }

            return currentValue;
        }
    }
}