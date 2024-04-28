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

        public MainMenuInitializer(Transform sceneCanvasTransform, IEntryPoint entryPoint, IUIFactory uiFactory)
        {
            _sceneCanvasTransform = sceneCanvasTransform;
            _entryPoint = entryPoint;
            _uiFactory = uiFactory;
        }

        public void Initialize()
        {
            _uiFactory.LoadScreen<MainMenu>(ScreenType.MainMenu, _sceneCanvasTransform);
            
            _entryPoint.HideLoadingScreen();
        }
    }
}