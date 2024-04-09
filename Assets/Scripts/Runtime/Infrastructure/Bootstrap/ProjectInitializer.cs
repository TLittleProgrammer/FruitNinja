using Runtime.Infrastructure.Game;
using Runtime.UI.Screens;
using UnityEngine;
using Zenject;

namespace Runtime.Infrastructure.Bootstrap
{
    public sealed class ProjectInitializer : MonoBehaviour
    {
        [Inject] private ScreenContainer _screenContainer;
        [Inject] private IGameStateMachine _gameStateMachine;
        
        private async void Awake()
        {
            await _screenContainer.AsyncInitialize();
            await _gameStateMachine.AsyncInitialize();
            
            _gameStateMachine.AsyncLoadScene(Constants.SceneNames.MainMenu);
        }
    }
}