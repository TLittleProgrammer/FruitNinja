using Runtime.Infrastructure.Factories;
using Runtime.Infrastructure.NotStateMachine;
using Runtime.UI.Screens;
using UnityEngine;
using Zenject;

namespace Runtime.Infrastructure.Bootstrap
{
    public sealed class MainMenuInitializer : IInitializable
    {
        private readonly Transform _sceneCanvasTransform;
        private readonly IGameStateMachine _gameStateMachine;
        private readonly IUIFactory _uiFactory;

        public MainMenuInitializer(Transform sceneCanvasTransform, IGameStateMachine gameStateMachine, IUIFactory uiFactory)
        {
            _sceneCanvasTransform = sceneCanvasTransform;
            _gameStateMachine = gameStateMachine;
            _uiFactory = uiFactory;
        }

        public void Initialize()
        {
            _uiFactory.LoadScreen<MainMenu>(ScreenType.MainMenu, _sceneCanvasTransform);
            
            _gameStateMachine.HideLoadingScreen();
        }
    }
}