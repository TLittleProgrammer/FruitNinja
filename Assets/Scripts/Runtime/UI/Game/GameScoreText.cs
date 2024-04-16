using System;
using System.Collections;
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
        private Coroutine _lastCurrentScoreCoroutine;
        private Coroutine _lastCurrentBestScoreCoroutine;
        
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

        private void OnScoreChanged(int newScore)
        {
            GoAnimateText(_currentScoreText, String.Empty, _currentScoreValue, newScore, _lastCurrentScoreCoroutine, false);
        }

        private void OnBestScoreChanged(int newBestScore)
        {
            GoAnimateText(_bestScoreText, BestScorePrefix, _currentBestScoreValue, newBestScore, _lastCurrentBestScoreCoroutine, true);
        }

        private void GoAnimateText(TMP_Text scoreText, string prefix, int currentValue, int targetValue, Coroutine coroutine, bool isBestScore)
        {
            if (coroutine is not null)
            {
                StopCoroutine(coroutine);
            }

            coroutine = StartCoroutine(ChangeScoreText(scoreText, prefix, currentValue, targetValue, isBestScore));
        }

        private IEnumerator ChangeScoreText(TMP_Text scoreText, string prefix, int currentValue, int targetValue, bool isBestScore)
        {
            for (float timer = 0; timer < MaxTimeInMillisecondToChangeString; timer += Time.deltaTime)
            {
                float lerpValue = timer / MaxTimeInMillisecondToChangeString;
                int scoreToDisplay = (int)Mathf.Lerp(currentValue, targetValue, lerpValue);

                if (isBestScore)
                {
                    _currentBestScoreValue = scoreToDisplay;
                }
                else
                {
                    _currentScoreValue = scoreToDisplay;
                }

                scoreText.text = prefix + scoreToDisplay.ToString();
                
                yield return null;
            }
        }
    }
}