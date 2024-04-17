using System;
using Runtime.Infrastructure.Game;
using Runtime.Infrastructure.UserData;
using TMPro;
using UnityEngine;
using Zenject;

namespace Runtime.UI.Loose
{
    public sealed class LooseScreenText : MonoBehaviour
    {
        [SerializeField] private TMP_Text _currentScoreText;
        [SerializeField] private TMP_Text _bestScoreText;
        
        private UserData _userData;
        private GameParameters _gameParameters;

        [Inject]
        private void Construct(UserData userData, GameParameters gameParameters)
        {
            _gameParameters = gameParameters;
            _userData = userData;
        }

        private void OnEnable()
        {
            _currentScoreText.text = String.Format(_currentScoreText.text, _gameParameters.CurrentScore);
            _bestScoreText.text    = String.Format(_bestScoreText.text, _userData.bestScore);
        }
    }
}