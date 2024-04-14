using Runtime.Infrastructure.NotStateMachine;
using Runtime.Infrastructure.UserData;
using Runtime.UI.Screens;
using UnityEngine;
using Zenject;

namespace Runtime.Infrastructure.Bootstrap
{
    public sealed class ProjectInitializer : MonoBehaviour
    {
        [Inject] private ScreenContainer _screenContainer;
        [Inject] private IGameStateMachine _gameStateMachine;
        [Inject] private UserDataSaveLoadService _userDataSaveLoadService;
        
        private async void Awake()
        {
            await _screenContainer.AsyncInitialize();
            await _gameStateMachine.AsyncInitialize();
            _userDataSaveLoadService.Load();
            
            _gameStateMachine.AsyncLoadScene(Constants.SceneNames.MainMenu);
        }
    }
}