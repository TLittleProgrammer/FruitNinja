using Runtime.Infrastructure.NotStateMachine;
using Runtime.Infrastructure.UserData;
using Runtime.StaticData.Installers;
using Runtime.UI.Screens;
using UnityEngine;
using Zenject;

namespace Runtime.Infrastructure.Bootstrap
{
    public sealed class ProjectInitializer : IInitializable
    {
        private readonly ProjectSettings _projectSettings;
        private readonly ScreenContainer _screenContainer;
        private readonly IEntryPoint _entryPoint;
        private readonly IUserDataSaveLoadService _userDataSaveLoadService;

        public ProjectInitializer(
            ProjectSettings projectSettings,
            ScreenContainer screenContainer,
            IEntryPoint entryPoint,
            IUserDataSaveLoadService userDataSaveLoadService)
        {
            _projectSettings = projectSettings;
            _screenContainer = screenContainer;
            _entryPoint = entryPoint;
            _userDataSaveLoadService = userDataSaveLoadService;
        }

        public async void Initialize()
        {
            QualitySettings.vSyncCount = _projectSettings.QualitySettingsVSyncCount;
            Application.targetFrameRate = _projectSettings.ApplicationTargetFrameCount;
            
            await _screenContainer.AsyncInitialize();
            await _entryPoint.AsyncInitialize();
            _userDataSaveLoadService.Load();

            _entryPoint.AsyncLoadScene(Constants.SceneNames.MainMenu);
        }
    }
}