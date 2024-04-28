using Runtime.Infrastructure.Containers;
using Runtime.Infrastructure.EntryPoint;
using Runtime.Infrastructure.Factories;
using Runtime.Infrastructure.Mouse;
using Runtime.Infrastructure.SlicableObjects;
using Runtime.Infrastructure.Slicer.SliceServices.HealthFlying;
using Runtime.Infrastructure.StateMachine;
using Runtime.Infrastructure.Trail;
using Runtime.UI.Screens;
using UnityEngine;
using Zenject;

namespace Runtime.Infrastructure.Bootstrap
{
    public sealed class GameInitializer : IInitializable
    {
        private const string PathToGameCameraPrefab = "Prefabs/GameCamera";
        private const string PathToTrail = "Prefabs/WorldObjects/Trail";
        
        private readonly Canvas _gameCanvas;
        private readonly Canvas _overlayCanvas;
        private readonly Transform _healthParent;
        private readonly SlicableVisualContainer _slicableVisualContainer;
        private readonly GameScreenManager _gameScreenManager;
        private readonly TrailMoveService _trailMoveService;
        private readonly IWorldFactory _worldFactory;
        private readonly IHealthFlyingService _healthFlyingService;
        private readonly SpriteProviderContainer _spriteProviderContainer;
        private readonly SlicableObjectView.Pool _slicableObjectViewPool;
        private readonly IGameStateMachine _gameStateMachine;
        private readonly MouseManager _mouseManager;
        private readonly IEntryPoint _entryPoint;
        private readonly IUIFactory _uiFactory;
        //TODO крайне плохое решение, стоит подумать и действительно посмотреть на Zenject фабрики 
        private readonly DiContainer _diContainer;

        public GameInitializer(
            Canvas gameCanvas,
            Canvas overlayCanvas,
            Transform healthParent,
            SlicableVisualContainer slicableVisualContainer,
            GameScreenManager gameScreenManager,
            TrailMoveService trailMoveService,
            MouseManager mouseManager,
            DiContainer diContainer,
            IEntryPoint entryPoint,
            IUIFactory uiFactory,
            IWorldFactory worldFactory,
            IHealthFlyingService healthFlyingService,
            SpriteProviderContainer spriteProviderContainer,
            SlicableObjectView.Pool slicableObjectViewPool,
            IGameStateMachine gameStateMachine
        )
        {
            _gameCanvas = gameCanvas;
            _overlayCanvas = overlayCanvas;
            _healthParent = healthParent;
            _slicableVisualContainer = slicableVisualContainer;
            _gameScreenManager = gameScreenManager;
            _trailMoveService = trailMoveService;
            _mouseManager = mouseManager;
            _diContainer = diContainer;
            _entryPoint = entryPoint;
            _uiFactory = uiFactory;
            _worldFactory = worldFactory;
            _healthFlyingService = healthFlyingService;
            _spriteProviderContainer = spriteProviderContainer;
            _slicableObjectViewPool = slicableObjectViewPool;
            _gameStateMachine = gameStateMachine;
        }

        public async void Initialize()
        {
            await _slicableVisualContainer.AsyncInitialize();
            await _spriteProviderContainer.AsyncInitialize();

            foreach (SlicableObjectView view in _slicableObjectViewPool.InactiveItems)
            {
                view.GetComponent<HideObjectAfterExitingScreen>()?.AsyncInitialize(_gameStateMachine);
            }
            
            TrailView trailView = await _worldFactory.CreateObject<TrailView>(PathToTrail, null);

            Camera camera = await _uiFactory.LoadUIObjectByPath<Camera>(PathToGameCameraPrefab, null, Vector3.back * 10);
            _gameCanvas.worldCamera = camera;

            await _mouseManager.AsyncInitialize(camera);
            await _trailMoveService.AsyncInitialize(trailView);

            _uiFactory.LoadScreen<MonoBehaviour>(ScreenType.GameBackground, _gameCanvas.transform, _diContainer);
            GameScreen gameScreen = _uiFactory.LoadScreen<GameScreen>(ScreenType.Game, _overlayCanvas.transform, _diContainer);
            await gameScreen.AsyncInitialize();
            _healthParent.transform.parent.SetAsLastSibling();

            _healthFlyingService.AsyncInitialize(gameScreen);

            await _gameScreenManager.AsyncInitialize(camera);
            
            _entryPoint.HideLoadingScreen();
        }
    }
}