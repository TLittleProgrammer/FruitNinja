using Runtime.Infrastructure.Factories;
using Runtime.Infrastructure.Game;
using Runtime.UI.Screens;
using UnityEngine;
using Zenject;

namespace Runtime.Infrastructure.Bootstrap
{
    public sealed class MainMenuInitializer : MonoBehaviour
    {
        [Inject] private IGameStateMachine _gameStateMachine;
        [Inject] private RootUI _rootUI;
        [Inject] private IUIFactory _uiFactory;
        
        private async void Awake()
        {
            _uiFactory.LoadScreen<MainMenu>(ScreenType.MainMenu, _rootUI.CanvasTransform);
            
            _gameStateMachine.HideLoadingScreen();
        }
    }
}