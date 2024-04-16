using Runtime.Infrastructure.NotStateMachine;
using Runtime.Infrastructure.UserData;
using Runtime.UI.Screens;
using UnityEngine;
using Zenject;

namespace Runtime.Infrastructure.Bootstrap
{
    public sealed class ProjectInitializer : IInitializable
    {
        private readonly ScreenContainer _screenContainer;
        private readonly IGameStateMachine _gameStateMachine;
        private readonly IUserDataSaveLoadService _userDataSaveLoadService;

        public ProjectInitializer(
            ScreenContainer screenContainer,
            IGameStateMachine gameStateMachine,
            IUserDataSaveLoadService userDataSaveLoadService)
        {
            _screenContainer = screenContainer;
            _gameStateMachine = gameStateMachine;
            _userDataSaveLoadService = userDataSaveLoadService;
        }

        public async void Initialize()
        {
            await _screenContainer.AsyncInitialize();
            await _gameStateMachine.AsyncInitialize();
            _userDataSaveLoadService.Load();

            _gameStateMachine.AsyncLoadScene(Constants.SceneNames.MainMenu);
        }
    }
}