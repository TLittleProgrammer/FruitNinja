using Runtime.Infrastructure.Effects;
using Runtime.Infrastructure.Factories;
using Runtime.Infrastructure.Mouse;
using Runtime.Infrastructure.SlicableObjects;
using Runtime.Infrastructure.SlicableObjects.Movement;
using Runtime.Infrastructure.SlicableObjects.Spawner;
using Runtime.StaticData.Installers;
using UnityEngine;
using Zenject;

namespace Runtime.Infrastructure.Bootstrap
{
    public sealed class GameInstaller : MonoInstaller
    {
        [SerializeField] private SlicableObjectView _slicableObjectViewPrefab;
        [SerializeField] private GameObject _poolParent;
        
        [SerializeField] private SliceableObjectDummy _dummyPrefab;
        [SerializeField] private GameObject _dummyPoolParent;
        
        [SerializeField] private BlotEffect _blotEffectPrefab;
        [SerializeField] private GameObject _blotPoolParent;

        [Inject] private PoolSettings _poolSettings;
        
        public override void InstallBindings()
        {
            Container.Bind<SlicableSpriteContainer>().AsSingle();
            Container.Bind<GameScreenManager>().AsSingle();
            Container.Bind<SlicableModelViewMapper>().AsSingle();
            Container.Bind<CanSliceResolver>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<MouseMoveService>().AsSingle();
            Container.BindInterfacesAndSelfTo<WorldFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<SlicableObjectSpawnerManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<SlicableMovementService>().AsSingle();
            Container.BindInterfacesAndSelfTo<MouseManager>().AsSingle();

            Container
                .BindMemoryPool<SlicableObjectView, SlicableObjectView.Pool>()
                .WithInitialSize(_poolSettings.PoolInitialSize)
                .FromComponentInNewPrefab(_slicableObjectViewPrefab)
                .UnderTransformGroup(_poolParent.name);
            
            Container
                .BindMemoryPool<SliceableObjectDummy, SliceableObjectDummy.Pool>()
                .WithInitialSize(_poolSettings.PoolInitialSize * 2)
                .FromComponentInNewPrefab(_dummyPrefab)
                .UnderTransformGroup(_dummyPoolParent.name);

            Container
                .BindMemoryPool<BlotEffect, BlotEffect.Pool>()
                .WithInitialSize(_poolSettings.PoolInitialSize * 2)
                .FromComponentInNewPrefab(_blotEffectPrefab)
                .UnderTransformGroup(_blotPoolParent.name);
        }
    }
}