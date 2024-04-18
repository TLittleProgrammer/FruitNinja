using Cysharp.Threading.Tasks;
using Runtime.Infrastructure;
using Runtime.Infrastructure.Factories;
using Runtime.Infrastructure.StateMachine;
using Runtime.UI.Game;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using PauseState = Runtime.Infrastructure.StateMachine.States.PauseState;

namespace Runtime.UI.Screens
{
    public sealed class GameScreen : MonoBehaviour, IAsyncInitializable
    {
        [SerializeField] private HeartsView _heartsView;
        [SerializeField] private Button _pauseButton;
        
        private IUIFactory _uiFactory;
        private IGameStateMachine _gameStateMachine;

        [Inject]
        private void Construct(IUIFactory uiFactory, IGameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
            _uiFactory = uiFactory;
            
            _pauseButton.onClick.AddListener(OnPauseButtonClicked);
        }

        public async UniTask AsyncInitialize()
        {
            await _heartsView.AsyncInitialize(_uiFactory);
        }

        private void OnPauseButtonClicked()
        {
            _gameStateMachine.Enter<PauseState>();
        }
    }
}