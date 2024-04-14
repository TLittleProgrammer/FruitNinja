using System.Collections;
using Runtime.Infrastructure.UserData;
using TMPro;
using UnityEngine;
using Zenject;

namespace Runtime.UI.MainMenu
{
    public class BestScoreText : MonoBehaviour
    {
        [SerializeField] private TMP_Text _bestScoreText;
        [SerializeField] private float _duration;

        private int _targetScore;
        private int _currentScore;
        
        [Inject]
        private void Construct(UserData userData)
        {
            _targetScore = userData.bestScore;
            _bestScoreText.text = userData.bestScore.ToString();
        }

        private void OnEnable()
        {
            if (_targetScore != 0)
            {
                StartCoroutine(ChangeBestScore());
            }
        }

        private IEnumerator ChangeBestScore()
        {
            float timeOffset = _duration / _targetScore;
            _currentScore = 0;

            while (_currentScore <= _targetScore)
            {
                _currentScore++;
                _bestScoreText.text = _currentScore.ToString();

                yield return new WaitForSeconds(timeOffset);
            }
        }
    }
}