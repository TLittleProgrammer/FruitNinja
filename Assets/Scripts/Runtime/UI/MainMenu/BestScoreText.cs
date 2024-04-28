using Runtime.Infrastructure.DOTweenAnimationServices.Score;
using Runtime.Infrastructure.UserData;
using TMPro;
using UnityEngine;
using Zenject;

namespace Runtime.UI.MainMenu
{
    public class BestScoreText : MonoBehaviour
    {
        [SerializeField] private TMP_Text _bestScoreText;

        private int _targetScore;
        private IScoreAnimationService _service;

        [Inject]
        private void Construct(UserData userData, IScoreAnimationService service)
        {
            _service = service;
            _targetScore = userData.bestScore;
            _bestScoreText.text = userData.bestScore.ToString();
        }

        private void OnEnable()
        {
            if (_targetScore != 0)
            {
                _service.Animate(_bestScoreText, 0, _targetScore);
            }
        }
    }
}