﻿using Runtime.Infrastructure.Factories;
using Runtime.Infrastructure.Mouse;
using Runtime.Infrastructure.NotStateMachine;
using Runtime.Infrastructure.SlicableObjects;
using Runtime.Infrastructure.Trail;
using Runtime.UI.Screens;
using TMPro;
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
        private readonly SlicableVisualContainer _slicableVisualContainer;
        private readonly GameScreenManager _gameScreenManager;
        private readonly TrailMoveService _trailMoveService;
        private readonly IWorldFactory _worldFactory;
        private readonly MouseManager _mouseManager;
        private readonly IGameStateMachine _gameStateMachine;
        private readonly IUIFactory _uiFactory;
        //TODO крайне плохое решение, стоит подумать и действительно посмотреть на Zenject фабрики 
        private readonly DiContainer _diContainer;

        public GameInitializer(
            Canvas gameCanvas,
            Canvas overlayCanvas,
            SlicableVisualContainer slicableVisualContainer,
            GameScreenManager gameScreenManager,
            TrailMoveService trailMoveService,
            MouseManager mouseManager,
            DiContainer diContainer,
            IGameStateMachine gameStateMachine,
            IUIFactory uiFactory,
            IWorldFactory worldFactory
        )
        {
            _gameCanvas = gameCanvas;
            _overlayCanvas = overlayCanvas;
            _slicableVisualContainer = slicableVisualContainer;
            _gameScreenManager = gameScreenManager;
            _trailMoveService = trailMoveService;
            _mouseManager = mouseManager;
            _diContainer = diContainer;
            _gameStateMachine = gameStateMachine;
            _uiFactory = uiFactory;
            _worldFactory = worldFactory;
        }

        public async void Initialize()
        {
            await _slicableVisualContainer.AsyncInitialize();
            
            TrailView trailView = await _worldFactory.CreateObject<TrailView>(PathToTrail, null);

            Camera camera = await _uiFactory.LoadUIObjectByPath<Camera>(PathToGameCameraPrefab, null, Vector3.back * 10);
            _gameCanvas.worldCamera = camera;

            await _mouseManager.AsyncInitialize(camera);
            await _trailMoveService.AsyncInitialize(trailView);

            _uiFactory.LoadScreen<MonoBehaviour>(ScreenType.GameBackground, _gameCanvas.transform, _diContainer);
            GameScreen gameScreen = _uiFactory.LoadScreen<GameScreen>(ScreenType.Game, _overlayCanvas.transform, _diContainer);
            await gameScreen.AsyncInitialize();

            await _gameScreenManager.AsyncInitialize(camera);
            
            _gameStateMachine.HideLoadingScreen();
        }
    }
}