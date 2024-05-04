using System.Collections.Generic;
using DG.Tweening;
using Runtime.Extensions;
using Runtime.Infrastructure.Combo;
using Runtime.Infrastructure.Containers;
using Runtime.Infrastructure.Effects;
using Runtime.Infrastructure.Factories;
using Runtime.Infrastructure.Game;
using Runtime.Infrastructure.Loose;
using Runtime.Infrastructure.Mouse;
using Runtime.Infrastructure.Score;
using Runtime.Infrastructure.SlicableObjects;
using Runtime.Infrastructure.SlicableObjects.CollisionDetector;
using Runtime.Infrastructure.SlicableObjects.MonoBehaviours;
using Runtime.Infrastructure.SlicableObjects.Movement;
using Runtime.Infrastructure.SlicableObjects.Services;
using Runtime.Infrastructure.SlicableObjects.Spawner;
using Runtime.Infrastructure.SlicableObjects.Spawner.SpawnCriterias;
using Runtime.Infrastructure.Slicer;
using Runtime.Infrastructure.Slicer.SliceServices;
using Runtime.Infrastructure.Slicer.SliceServices.HealthFlying;
using Runtime.Infrastructure.Slicer.SliceServices.Helpers;
using Runtime.Infrastructure.StateMachine;
using Runtime.Infrastructure.StateMachine.States;
using Runtime.Infrastructure.Timer;
using Runtime.Infrastructure.Trail;
using Runtime.StaticData.Blur;
using Runtime.StaticData.Installers;
using Runtime.StaticData.Level;
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
        
        [SerializeField] private FlyingHealthView _healthFlyingViewPrefab;
        [SerializeField] private Transform _healthFlyingPoolParent;
        
        [SerializeField] private HeartSplash _heartSplashPrefab;
        [SerializeField] private Transform _heartPoolParent;
        
        [SerializeField] private BombEffect _bombEffectPrefab;
        [SerializeField] private Transform _bombEffectPoolParent;
        
        [SerializeField] private BlurSettings _blurSettings;
        
        [Inject] private PoolSettings _poolSettings;
        [Inject] private LevelStaticData _levelStaticData;

        public override void InstallBindings()
        {
            DOTween.SetTweensCapacity(500, 500);
            Container.BindInterfacesTo<GameInstaller>().FromInstance(this).AsSingle();

            Container.Bind<SpriteProviderContainer>().AsSingle();
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
            Container.Bind<ISpawnCriteriaService>().To<SpawnCriteriaService>().AsSingle();
            Container.Bind<ICreateDummiesService>().To<CreateDummiesService>().AsSingle();
            Container.Bind<IHealthFlyingService>().To<HealthFlyingService>().AsSingle();
            Container.Bind<ISplashBombService>().To<SplashBombService>().AsSingle();
            Container.Bind<IMimikService>().To<MimikService>().AsSingle();
            Container.Bind<ITimeProvider>().To<TimeProvider>().AsSingle();

            Container.BindInterfacesAndSelfTo<Stopwatch>().AsSingle();
            Container.BindInterfacesAndSelfTo<TrailMoveService>().AsSingle();
            Container.BindInterfacesAndSelfTo<WorldFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<SlicableMovementService>().AsSingle();
            Container.BindInterfacesAndSelfTo<MouseManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<ComboService>().AsSingle();
            Container.BindInterfacesAndSelfTo<Timer.Timer>().AsSingle();

            ISlicer slicer = Container.Instantiate<Slicer.Slicer>();
            ICollisionDetector<Collider2D, SlicableObjectView> collisionDetector = Container.Instantiate<CollisionDetector>(new[] { slicer });

            Container.BindInterfacesAndSelfTo<CollisionDetector>().FromInstance(collisionDetector).AsSingle();

            IGameStateMachine gameStateMachine = Container.Instantiate<GameStateMachine>();
            
            Container.BindPool<BombEffect, BombEffect.Pool>(_poolSettings.PoolInitialSize / 2, _bombEffectPrefab, _bombEffectPoolParent);
            Container.BindPool<SlicableObjectView, SlicableObjectView.Pool>(_poolSettings.PoolInitialSize, _slicableObjectViewPrefab, _poolParent);
            Container.BindPool<ScoreEffect, ScoreEffect.Pool>(_poolSettings.PoolInitialSize, _scoreEffectPrefab, _scorePoolParent);
            Container.BindPool<SplashEffect, SplashEffect.Pool>(_poolSettings.PoolInitialSize, _splashEffectPrefab, _splashPoolParent);
            Container.BindPool<SliceableObjectDummy, SliceableObjectDummy.Pool>(_poolSettings.PoolInitialSize * 2, _dummyPrefab, _dummyPoolParent);
            Container.BindPool<BlotEffect, BlotEffect.Pool>(_poolSettings.PoolInitialSize * 2, _blotEffectPrefab, _blotPoolParent);
            Container.BindPool<ComboView, ComboView.Pool>(_poolSettings.PoolInitialSize, _comboViewPrefab, _comboPoolParent);
            Container.BindPool<FlyingHealthView, FlyingHealthView.Pool>(_levelStaticData.HealthCount, _healthFlyingViewPrefab, _healthFlyingPoolParent);
            Container.BindPool<HeartSplash, HeartSplash.Pool>(_levelStaticData.HealthCount, _heartSplashPrefab, _heartPoolParent);
            
            SlicableObjectSpawnerManager slicableObjectSpawnerManager = Container.Instantiate<SlicableObjectSpawnerManager>(new[] { gameStateMachine });
            
            Dictionary<SlicableObjectType, ISliceService> sliceServices = GetSliceServices(gameStateMachine);

            slicer.AsyncInitialize(sliceServices);
            Container.BindInterfacesAndSelfTo<Slicer.Slicer>().FromInstance(slicer).AsSingle();

            InstallGameStateMachine(gameStateMachine, slicableObjectSpawnerManager);
            InstallAndBindLooseService();
            
            Container.BindInterfacesAndSelfTo<SlicableObjectSpawnerManager>().FromInstance(slicableObjectSpawnerManager).AsSingle();

            Container.Bind<ComboSequenceResolver>().AsSingle();
            Container.Bind<GameInitializer>().AsSingle().WithArguments(_gameCanvas, _overlayCanvas, _healthFlyingPoolParent.transform);
        }

        private Dictionary<SlicableObjectType, ISliceService> GetSliceServices(IGameStateMachine gameStateMachine)
        {
            Dictionary<SlicableObjectType, ISliceService> sliceServices = new();

            sliceServices.Add(SlicableObjectType.Simple, Container.Instantiate<SimpleSliceService>());
            sliceServices.Add(SlicableObjectType.Brick, Container.Instantiate<BrickSliceService>());
            sliceServices.Add(SlicableObjectType.Health, Container.Instantiate<HealthSliceService>());
            sliceServices.Add(SlicableObjectType.Bomb, Container.Instantiate<BombSliceService>());
            sliceServices.Add(SlicableObjectType.Avosjka, Container.Instantiate<AvosjkaSliceService>());
            sliceServices.Add(SlicableObjectType.Samurai, Container.Instantiate<SamuraiSliceService>(new object[] { gameStateMachine }));
            sliceServices.Add(SlicableObjectType.Ice, Container.Instantiate<IceSliceService>(new object[] { _gameCanvas.transform, gameStateMachine }));

            MagnetSliceService magnetSliceService = Container.Instantiate<MagnetSliceService>(new[] { gameStateMachine });
            Container.BindInterfacesAndSelfTo<MagnetSliceService>().FromInstance(magnetSliceService).AsSingle();
            
            sliceServices.Add(SlicableObjectType.Magnet, magnetSliceService);

            return sliceServices;
        }

        private void InstallGameStateMachine(IGameStateMachine stateMachine, SlicableObjectSpawnerManager slicableObjectSpawnerManager)
        {
            IEnumerable<IState> states = GetGameStates(slicableObjectSpawnerManager, stateMachine);
            
            stateMachine.AsyncInitialize(states);

            Container.Bind<IGameStateMachine>().FromInstance(stateMachine).AsSingle();
        }

        
        //TODO Шаблонный код. Вынести в методы
        private IEnumerable<IState> GetGameStates(SlicableObjectSpawnerManager slicableObjectSpawnerManager, IGameStateMachine gameStateMachine)
        {
            List<IState> states = new();

            LooseState looseState = Container.Instantiate<LooseState>(new object[] { _overlayCanvas, slicableObjectSpawnerManager });
            Container.BindInterfacesTo<LooseState>().FromInstance(looseState).AsSingle();
            
            PauseState pauseState = Container.Instantiate<PauseState>(new object[] { _overlayCanvas, slicableObjectSpawnerManager });
            Container.Bind<PauseState>().FromInstance(pauseState).AsSingle();
            
            SamuraiState samuraiState = Container.Instantiate<SamuraiState>(new object[] { _gameCanvas.transform, gameStateMachine, slicableObjectSpawnerManager });
            Container.Bind<SamuraiState>().FromInstance(samuraiState).AsSingle();
            
            states.Add(Container.Instantiate<GameState>());
            states.Add(Container.Instantiate<RestartState>());
            states.Add(looseState);
            states.Add(pauseState);
            states.Add(samuraiState);

            return states;
        }

        private void InstallAndBindLooseService()
        {
            LooseService looseService = Container.Instantiate<LooseService>();

            Container.BindInterfacesAndSelfTo<LooseService>().FromInstance(looseService);
        }

        public void Initialize()
        {
            Container.Resolve<GameInitializer>().Initialize();
        }
    }
}