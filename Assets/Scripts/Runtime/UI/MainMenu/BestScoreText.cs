using Runtime.Infrastructure.UserData;
using TMPro;
using UnityEngine;
using Zenject;

namespace Runtime.UI.MainMenu
{
    public class BestScoreText : MonoBehaviour
    {
        [SerializeField] private TMP_Text _bestScoreText;

        [Inject]
        private void Construct(UserData userData)
        {
            _bestScoreText.text = userData.BestScore.ToString();
        }
    }
}