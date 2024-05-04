using DG.Tweening;
using Runtime.Constants;
using Runtime.Infrastructure.EntryPoint;
using Runtime.Infrastructure.UserData;
using Runtime.StaticData.Installers;
using Runtime.UI.Screens;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Runtime.Infrastructure.Bootstrap
{
    public sealed class ProjectInitializer : IInitializable
    {
        private readonly ProjectSettings _projectSettings;
        private readonly ScreenContainer _screenContainer;
        private readonly IEntryPoint _entryPoint;
        private readonly IUserDataSaveLoadService _userDataSaveLoadService;

        public bool ProjectInitialized = false;

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

            ProjectInitialized = true;
            DOTween.SetTweensCapacity(500, 500);

            if (SceneManager.GetActiveScene().name.Equals(SceneNames.Bootstrap))
            {
                _entryPoint.AsyncLoadScene(SceneNames.MainMenu);
            }
        }
    }
}