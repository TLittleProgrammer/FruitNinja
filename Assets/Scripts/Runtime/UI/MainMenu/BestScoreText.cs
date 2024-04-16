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
                StartCoroutine(ChangeBestScoreText());
            }
        }

        private IEnumerator ChangeBestScoreText()
        {
            for (float timer = 0; timer < _duration; timer += Time.deltaTime)
            {
                float lerpValue = timer / _duration;
                int scoreToDisplay = (int)Mathf.Lerp(0, _targetScore, lerpValue);

                _bestScoreText.text = scoreToDisplay.ToString();
                
                yield return null;
            }
        }
    }
}