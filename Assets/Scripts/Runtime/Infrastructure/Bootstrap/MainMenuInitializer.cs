using Cysharp.Threading.Tasks;
using Runtime.Infrastructure.EntryPoint;
using Runtime.Infrastructure.Factories;
using Runtime.UI.Screens;
using UnityEngine;
using Zenject;

namespace Runtime.Infrastructure.Bootstrap
{
    public sealed class MainMenuInitializer : IInitializable
    {
        private readonly Transform _sceneCanvasTransform;
        private readonly IEntryPoint _entryPoint;
        private readonly IUIFactory _uiFactory;
        private readonly ProjectInitializer _projectInitializer;

        public MainMenuInitializer(Transform sceneCanvasTransform, IEntryPoint entryPoint, IUIFactory uiFactory, ProjectInitializer projectInitializer)
        {
            _sceneCanvasTransform = sceneCanvasTransform;
            _entryPoint = entryPoint;
            _uiFactory = uiFactory;
            _projectInitializer = projectInitializer;
        }

        public async void Initialize()
        {
            while (_projectInitializer.ProjectInitialized is false)
            {
                await UniTask.Delay(20);
            }
            
            _uiFactory.LoadScreen<MainMenu>(ScreenType.MainMenu, _sceneCanvasTransform);
            
            _entryPoint.HideLoadingScreen();
        }
    }
}