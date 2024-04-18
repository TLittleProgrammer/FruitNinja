using Runtime.Constants;
using Runtime.Infrastructure.NotStateMachine;
using Runtime.Infrastructure.StateMachine;
using Runtime.Infrastructure.StateMachine.States;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Runtime.UI.Screens
{
    public class PauseScreen : MonoBehaviour
    {
        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _menuButton;
        
        private IEntryPoint _entryPoint;
        private IGameStateMachine _gameStateMachine;

        [Inject]
        private void Construct(IEntryPoint entryPoint, IGameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
            _entryPoint = entryPoint;
        }

        private void OnEnable()
        {
            _menuButton.onClick.AddListener(OnMenuButtonClicked);
            _continueButton.onClick.AddListener(OnContinueButtonClicked);
        }

        private void OnDisable()
        {
            _menuButton.onClick.RemoveListener(OnMenuButtonClicked);
            _continueButton.onClick.RemoveListener(OnContinueButtonClicked);
        }

        private void OnContinueButtonClicked()
        {
            _gameStateMachine.Enter<GameState>();
        }

        private void OnMenuButtonClicked()
        {
            _entryPoint.AsyncLoadScene(SceneNames.MainMenu);
        }
    }
}