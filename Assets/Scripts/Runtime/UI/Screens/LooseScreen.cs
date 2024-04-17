using Runtime.Constants;
using Runtime.Infrastructure.Game;
using Runtime.Infrastructure.NotStateMachine;
using Runtime.Infrastructure.SlicableObjects.Movement;
using Runtime.Infrastructure.SlicableObjects.Spawner;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Runtime.UI.Screens
{
    public sealed class LooseScreen : MonoBehaviour
    {
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _menuButton;
        
        private IGameStateMachine _gameStateMachine;
        private SlicableMovementService _movementService;
        private SlicableObjectSpawnerManager _spawnerManager;
        private GameParameters _gameParameters;

        [Inject]
        private void Construct(
            IGameStateMachine gameStateMachine,
            SlicableMovementService movementService,
            SlicableObjectSpawnerManager spawnerManager,
            GameParameters gameParameters
        )
        {
            _gameParameters = gameParameters;
            _spawnerManager = spawnerManager;
            _movementService = movementService;
            _gameStateMachine = gameStateMachine;
        }

        private void OnEnable()
        {
            _movementService.Stop();
            _spawnerManager.Stop();
            
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
            _movementService.Reset();
            _spawnerManager.Continue();
            
            _gameParameters.Reset();
            
            //TODO он не должен отвечать за свй жизненный цикл, но пусть пока будет так
            Destroy(gameObject);
        }

        private void OnMenuButtonClicked()
        {
            _gameStateMachine.AsyncLoadScene(SceneNames.MainMenu);
        }
    }
}