using Runtime.Extensions;
using Runtime.Infrastructure.Effects;
using Runtime.Infrastructure.Factories;
using Runtime.Infrastructure.Game;
using Runtime.Infrastructure.Mouse;
using Runtime.Infrastructure.SlicableObjects;
using Runtime.Infrastructure.SlicableObjects.Movement;
using Runtime.Infrastructure.SlicableObjects.Spawner;
using Runtime.StaticData.Installers;
using UnityEngine;
using Zenject;

namespace Runtime.Infrastructure.Bootstrap
{
    public sealed class GameInstaller : MonoInstaller, IInitializable
    {
        [SerializeField] private Canvas _gameCanvas;
        
        [SerializeField] private SlicableObjectView _slicableObjectViewPrefab;
        [SerializeField] private GameObject _poolParent;
        
        [SerializeField] private SliceableObjectDummy _dummyPrefab;
        [SerializeField] private GameObject _dummyPoolParent;
        
        [SerializeField] private BlotEffect _blotEffectPrefab;
        [SerializeField] private GameObject _blotPoolParent;
        
        [SerializeField] private SplashEffect _splashEffectPrefab;
        [SerializeField] private GameObject _splashPoolParent;

        [Inject] private PoolSettings _poolSettings;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<GameInstaller>().FromInstance(this).AsSingle();
            
            Container.Bind<GameParameters>().AsSingle();
            Container.Bind<SlicableVisualContainer>().AsSingle();
            Container.Bind<GameScreenManager>().AsSingle();
            Container.Bind<SlicableModelViewMapper>().AsSingle();
            Container.Bind<CanSliceResolver>().AsSingle();

            Container.BindInterfacesAndSelfTo<MouseMoveService>().AsSingle();
            Container.BindInterfacesAndSelfTo<WorldFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<SlicableObjectSpawnerManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<SlicableMovementService>().AsSingle();
            Container.BindInterfacesAndSelfTo<MouseManager>().AsSingle();

            Container.BindPool<SlicableObjectView, SlicableObjectView.Pool>(_poolSettings.PoolInitialSize, _slicableObjectViewPrefab, _poolParent.name);
            Container.BindPool<SplashEffect, SplashEffect.Pool>(_poolSettings.PoolInitialSize, _splashEffectPrefab, _splashPoolParent.name);
            Container.BindPool<SliceableObjectDummy, SliceableObjectDummy.Pool>(_poolSettings.PoolInitialSize * 2, _dummyPrefab, _dummyPoolParent.name);
            Container.BindPool<BlotEffect, BlotEffect.Pool>(_poolSettings.PoolInitialSize * 2, _blotEffectPrefab, _blotPoolParent.name);
        }

        public void Initialize()
        {
            GameInitializer gameInitializer = Container.Instantiate<GameInitializer>(new[] { _gameCanvas });
            
            gameInitializer.Initialize();
        }
    }
}