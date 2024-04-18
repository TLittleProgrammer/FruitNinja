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
        
        private IEntryPoint _entryPoint;
        private SlicableMovementService _movementService;
        private SlicableObjectSpawnerManager _spawnerManager;
        private GameParameters _gameParameters;

        [Inject]
        private void Construct(
            IEntryPoint entryPoint,
            SlicableMovementService movementService,
            SlicableObjectSpawnerManager spawnerManager,
            GameParameters gameParameters
        )
        {
            _gameParameters = gameParameters;
            _spawnerManager = spawnerManager;
            _movementService = movementService;
            _entryPoint = entryPoint;
        }

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
            _spawnerManager.Continue();
            
            _gameParameters.Reset();
            
            //TODO он не должен отвечать за свй жизненный цикл, но пусть пока будет так
            Destroy(gameObject);
        }

        private void OnMenuButtonClicked()
        {
            _entryPoint.AsyncLoadScene(SceneNames.MainMenu);
        }
    }
}