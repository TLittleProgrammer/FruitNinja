using Runtime.Constants;
using Runtime.Infrastructure.NotStateMachine;
using Runtime.Infrastructure.StateMachine;
using Runtime.Infrastructure.StateMachine.States;
using Runtime.UI.Buttons;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Runtime.UI.Screens
{
    public class PauseScreen : MonoBehaviour
    {
        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _menuButton;
        [SerializeField] private AnimatableButton[] _animatableButtons;
        [SerializeField] private Image _background;
        [SerializeField] private TMP_Text _label;

        private IEntryPoint _entryPoint;
        private IGameStateMachine _gameStateMachine;

        [Inject]
        private void Construct(IEntryPoint entryPoint, IGameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
            _entryPoint = entryPoint;
        }

        public Transform[] Transforms => new[] { _label.transform, _menuButton.transform , _continueButton.transform};
        public Image Background => _background;
        
        private void OnEnable()
        {
            _menuButton.onClick.AddListener(OnMenuButtonClicked);
            _continueButton.onClick.AddListener(OnContinueButtonClicked);
        }

        private void OnDisable()
        {
            DisableButtons();
        }

        private void OnContinueButtonClicked()
        {
            DisableButtons();
            
            _gameStateMachine.Enter<GameState>();
        }

        private void OnMenuButtonClicked()
        {
            if (_gameStateMachine.CurrentState is GameState)
            {
                return;
            }
            DisableButtons();

            _entryPoint.AsyncLoadScene(SceneNames.MainMenu);
        }

        private void DisableButtons()
        {
            _menuButton.onClick.RemoveListener(OnMenuButtonClicked);
            _continueButton.onClick.RemoveListener(OnContinueButtonClicked);

            foreach (AnimatableButton button in _animatableButtons)
            {
                button.enabled = false;
            }
        }
    }
}