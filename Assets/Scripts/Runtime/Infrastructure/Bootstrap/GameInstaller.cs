﻿using System.Collections.Generic;
using Runtime.Extensions;
using Runtime.Infrastructure.Combo;
using Runtime.Infrastructure.Effects;
using Runtime.Infrastructure.Factories;
using Runtime.Infrastructure.Game;
using Runtime.Infrastructure.Loose;
using Runtime.Infrastructure.Mouse;
using Runtime.Infrastructure.Score;
using Runtime.Infrastructure.SlicableObjects;
using Runtime.Infrastructure.SlicableObjects.CollisionDetector;
using Runtime.Infrastructure.SlicableObjects.Movement;
using Runtime.Infrastructure.SlicableObjects.Spawner;
using Runtime.Infrastructure.Slicer.SliceServices;
using Runtime.Infrastructure.StateMachine;
using Runtime.Infrastructure.StateMachine.States;
using Runtime.Infrastructure.Trail;
using Runtime.StaticData.Installers;
using UnityEngine;
using Zenject;

namespace Runtime.Infrastructure.Bootstrap
{
    public sealed class GameInstaller : MonoInstaller, IInitializable
    {
        [SerializeField] private Canvas _gameCanvas;
        [SerializeField] private Canvas _overlayCanvas;
        
        [SerializeField] private SlicableObjectView _slicableObjectViewPrefab;
        [SerializeField] private Transform _poolParent;
        
        [SerializeField] private SliceableObjectDummy _dummyPrefab;
        [SerializeField] private Transform _dummyPoolParent;
        
        [SerializeField] private BlotEffect _blotEffectPrefab;
        [SerializeField] private Transform _blotPoolParent;
        
        [SerializeField] private SplashEffect _splashEffectPrefab;
        [SerializeField] private Transform _splashPoolParent;
        
        [SerializeField] private ScoreEffect _scoreEffectPrefab;
        [SerializeField] private Transform _scorePoolParent;
        
        [SerializeField] private ComboView _comboViewPrefab;
        [SerializeField] private Transform _comboPoolParent;

        [Inject] private PoolSettings _poolSettings;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<GameInstaller>().FromInstance(this).AsSingle();


            Container.Bind<GameParameters>().AsSingle();
            Container.Bind<SlicableVisualContainer>().AsSingle();
            Container.Bind<GameScreenManager>().AsSingle();
            Container.Bind<SlicableModelViewMapper>().AsSingle();
            Container.Bind<SliceableObjectSpriteRendererOrderService>().AsSingle();
            Container.Bind<IIntermediateMousePositionsService>().To<IntermediateMousePositionsService>().AsSingle();
            Container.Bind<IShowEffectsService>().To<ShowEffectsService>().AsSingle();
            Container.Bind<IAddScoreService>().To<AddScoreService>().AsSingle();
            Container.Bind<IComboViewPositionCorrecter>().To<ComboViewPositionCorrecter>().AsSingle();
            Container.Bind<ISlicableObjectCounterOnMap>().To<SlicableObjectCounterOnMap>().AsSingle();

            Container.BindInterfacesAndSelfTo<TrailMoveService>().AsSingle();
            Container.BindInterfacesAndSelfTo<WorldFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<SlicableObjectSpawnerManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<SlicableMovementService>().AsSingle();
            Container.BindInterfacesAndSelfTo<MouseManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<CollisionDetector>().AsSingle();
            Container.BindInterfacesAndSelfTo<ComboService>().AsSingle();

            Container.BindPool<SlicableObjectView, SlicableObjectView.Pool>(_poolSettings.PoolInitialSize, _slicableObjectViewPrefab, _poolParent);
            Container.BindPool<ScoreEffect, ScoreEffect.Pool>(_poolSettings.PoolInitialSize, _scoreEffectPrefab, _scorePoolParent);
            Container.BindPool<SplashEffect, SplashEffect.Pool>(_poolSettings.PoolInitialSize, _splashEffectPrefab, _splashPoolParent);
            Container.BindPool<SliceableObjectDummy, SliceableObjectDummy.Pool>(_poolSettings.PoolInitialSize * 2, _dummyPrefab, _dummyPoolParent);
            Container.BindPool<BlotEffect, BlotEffect.Pool>(_poolSettings.PoolInitialSize * 2, _blotEffectPrefab, _blotPoolParent);
            Container.BindPool<ComboView, ComboView.Pool>(_poolSettings.PoolInitialSize, _comboViewPrefab, _comboPoolParent);
            
            Dictionary<SlicableObjectType, ISliceService> sliceServices = GetSliceServices();
            
            Container.Bind<Dictionary<SlicableObjectType, ISliceService>>().FromInstance(sliceServices).AsSingle();
            Container.BindInterfacesAndSelfTo<Slicer.Slicer>().AsSingle();

            InstallGameStateMachine();
            InstallAndBindLooseService();
        }

        private Dictionary<SlicableObjectType, ISliceService> GetSliceServices()
        {
            Dictionary<SlicableObjectType, ISliceService> sliceServices = new();

            sliceServices.Add(SlicableObjectType.Simple, Container.Instantiate<SimpleSliceService>());
            sliceServices.Add(SlicableObjectType.Brick, Container.Instantiate<BrickSliceService>());
            
            return sliceServices;
        }

        private void InstallGameStateMachine()
        {
            IGameStateMachine gameStateMachine;
            
            IEnumerable<IState> states = GetGameStates();
            
            gameStateMachine = new GameStateMachine(states);

            Container.Bind<IGameStateMachine>().FromInstance(gameStateMachine).AsSingle();
        }

        
        //TODO Шаблонный код. Вынести в методы
        private IEnumerable<IState> GetGameStates()
        {
            List<IState> states = new();

            LooseState looseState = Container.Instantiate<LooseState>(new[] { _overlayCanvas });
            Container.BindInterfacesTo<LooseState>().FromInstance(looseState).AsSingle();
            
            PauseState pauseState = Container.Instantiate<PauseState>(new[] { _overlayCanvas });
            Container.Bind<PauseState>().FromInstance(pauseState).AsSingle();
            
            states.Add(Container.Instantiate<GameState>());
            states.Add(Container.Instantiate<RestartState>());
            states.Add(looseState);
            states.Add(pauseState);

            return states;
        }

        private void InstallAndBindLooseService()
        {
            LooseService looseService = Container.Instantiate<LooseService>();

            Container.BindInterfacesAndSelfTo<LooseService>().FromInstance(looseService);
        }

        public void Initialize()
        {
            GameInitializer gameInitializer = Container.Instantiate<GameInitializer>(new[] { _gameCanvas, _overlayCanvas });
            
            gameInitializer.Initialize();
        }
    }
}