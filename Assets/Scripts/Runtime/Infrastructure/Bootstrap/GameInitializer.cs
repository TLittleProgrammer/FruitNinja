using Runtime.Infrastructure.Factories;
using Runtime.Infrastructure.Mouse;
using Runtime.Infrastructure.SlicableObjects;
using Runtime.UI.Screens;
using UnityEngine;
using Zenject;

namespace Runtime.Infrastructure.Bootstrap
{
    public sealed class GameInitializer : AbstractIntializer
    {
        private const string PathToGameCameraPrefab = "Prefabs/GameCamera";
        private const string PathToTrail = "Prefabs/WorldObjects/Trail";
        
        [SerializeField] private Canvas _gameCanvas;
        
        [Inject] private SlicableVisualContainer _slicableVisualContainer;
        [Inject] private GameScreenManager _gameScreenManager;
        [Inject] private MouseMoveService _mouseMoveService;
        [Inject] private IWorldFactory _worldFactory;
        [Inject] private MouseManager _mouseManager;
        
        //TODO крайне плохое решение, стоит подумать и действительно посмотреть на Zenject фабрики 
        [Inject] private DiContainer _diContainer;
        
        //TODO Когда будет свободное время - попробовать перенести в друое место инициализацию
        private async void Awake()
        {
            await _slicableVisualContainer.AsyncInitialize();
            
            Trail trail = await _worldFactory.CreateObject<Trail>(PathToTrail, null);

            Camera camera = await UiFactory.LoadUIObjectByPath<Camera>(PathToGameCameraPrefab, null, Vector3.back * 10);
            _gameCanvas.worldCamera = camera;

            await _mouseManager.AsyncInitialize(camera);
            await _mouseMoveService.AsyncInitialize(trail);

            GameScreen gameScreen = UiFactory.LoadScreen<GameScreen>(ScreenType.Game, SceneCanvasTransform, _diContainer);
            await gameScreen.AsyncInitialize();
            
            await _gameScreenManager.AsyncInitialize(camera);
            
            GameStateMachine.HideLoadingScreen();
        }
    }
}