using Runtime.Constants;
using Runtime.Infrastructure.NotStateMachine;
using Runtime.Infrastructure.StateMachine;
using Runtime.Infrastructure.StateMachine.States;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Runtime.UI.Screens
{
    public sealed class LooseScreen : MonoBehaviour
    {
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _menuButton;
        [SerializeField] private Image _background;
        [SerializeField] private Transform _allInfo;

        private IEntryPoint _entryPoint;
        private IGameStateMachine _gameStateMachine;

        [Inject]
        private void Construct(IEntryPoint entryPoint, IGameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
            _entryPoint = entryPoint;
        }

        public Image Background => _background;
        public Transform AllInfo => _allInfo;

        private void OnEnable()
        {
            _menuButton.onClick.AddListener(OnMenuButtonClicked);
            _restartButton.onClick.AddListener(OnRestartButtonClicked);
        }

        private void OnDisable()
        {
            _menuButton.onClick.RemoveListener(OnMenuButtonClicked);
            _restartButton.onClick.RemoveListener(OnRestartButtonClicked);
        }

        private void OnRestartButtonClicked()
        {
            _gameStateMachine.Enter<RestartState>();
            _gameStateMachine.Enter<GameState>();
        }

        private void OnMenuButtonClicked()
        {
            _entryPoint.AsyncLoadScene(SceneNames.MainMenu);
        }
    }
}