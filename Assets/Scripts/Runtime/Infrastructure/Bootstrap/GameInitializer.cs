using Runtime.SlicableObjects;
using Runtime.UI.Screens;
using UnityEngine;
using Zenject;

namespace Runtime.Infrastructure.Bootstrap
{
    public sealed class GameInitializer : AbstractIntializer
    {
        private const string PathToGameCameraPrefab = "Prefabs/GameCamera";
        
        [SerializeField] private Canvas _gameCanvas;
        
        [Inject] private SlicableSpriteContainer _slicableSpriteContainer;
        [Inject] private GameScreenPositionResolver _gameScreenPositionResolver;
        
        private async void Awake()
        {
            await _slicableSpriteContainer.AsyncInitialize();
            
            Camera camera = await UiFactory.LoadUIObjectByPath<Camera>(PathToGameCameraPrefab, null, Vector3.back * 10);
            _gameCanvas.worldCamera = camera;
            
            UiFactory.LoadScreen<GameScreen>(ScreenType.Game, SceneCanvasTransform);
            await _gameScreenPositionResolver.AsyncInitialize(camera);
            
            GameStateMachine.HideLoadingScreen();
        }
    }
}