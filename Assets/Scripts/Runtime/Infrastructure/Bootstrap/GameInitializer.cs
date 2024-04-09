using Runtime.Infrastructure.Factories;
using Runtime.Infrastructure.Game;
using Runtime.UI.Screens;
using UnityEngine;
using Zenject;

namespace Runtime.Infrastructure.Bootstrap
{
    public sealed class GameInitializer : MonoBehaviour
    {
        [Inject] private IGameStateMachine _gameStateMachine;
        [Inject] private RootUI _rootUI;
        [Inject] private IUIFactory _uiFactory;

        private async void Awake()
        {
            _uiFactory.LoadScreen<GameScreen>(ScreenType.Game, _rootUI.CanvasTransform);
            
            _gameStateMachine.HideLoadingScreen();
        }
    }
}