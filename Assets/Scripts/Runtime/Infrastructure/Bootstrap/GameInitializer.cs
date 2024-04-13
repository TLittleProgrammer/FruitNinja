using Runtime.Infrastructure.Factories;
using Runtime.Infrastructure.Mouse;
using Runtime.SlicableObjects;
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
        
        [Inject] private SlicableSpriteContainer _slicableSpriteContainer;
        [Inject] private GameScreenPositionResolver _gameScreenPositionResolver;
        [Inject] private MouseMoveService _mouseMoveService;
        [Inject] private IWorldFactory _worldFactory;
        
        //TODO Когда будет свободное время - попробовать перенести в друое место инициализацию
        private async void Awake()
        {
            await _slicableSpriteContainer.AsyncInitialize();
            
            Trail trail = await _worldFactory.CreateObject<Trail>(PathToTrail, null);

            Camera camera = await UiFactory.LoadUIObjectByPath<Camera>(PathToGameCameraPrefab, null, Vector3.back * 10);
            _gameCanvas.worldCamera = camera;

            _mouseMoveService.AsyncInitialize(trail, camera);
            
            UiFactory.LoadScreen<GameScreen>(ScreenType.Game, SceneCanvasTransform);
            await _gameScreenPositionResolver.AsyncInitialize(camera);
            
            GameStateMachine.HideLoadingScreen();
        }
    }
}