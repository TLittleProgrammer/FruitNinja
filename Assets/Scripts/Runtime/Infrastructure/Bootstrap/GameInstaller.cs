using Runtime.Infrastructure.Factories;
using Runtime.SlicableObjects;
using Runtime.SlicableObjects.Movement;
using Runtime.SlicableObjects.Spawner;
using Runtime.StaticData.Installers;
using UnityEngine;
using Zenject;

namespace Runtime.Infrastructure.Bootstrap
{
    public sealed class GameInstaller : MonoInstaller
    {
        [SerializeField] private SlicableObjectView _slicableObjectViewPrefab;
        [SerializeField] private GameObject _poolParent;

        [Inject] private PoolSettings _poolSettings;
        
        public override void InstallBindings()
        {
            Container.Bind<SlicableSpriteContainer>().AsSingle();
            Container.Bind<GameScreenPositionResolver>().AsSingle();
            Container.Bind<SlicableModelViewMapper>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<WorldFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<SlicableObjectSpawnerManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<SlicableMovementService>().AsSingle();

            Container
                .BindMemoryPool<SlicableObjectView, SlicableObjectView.Pool>()
                .WithInitialSize(_poolSettings.PoolInitialSize)
                .FromComponentInNewPrefab(_slicableObjectViewPrefab)
                .UnderTransformGroup(_poolParent.name);
        }
    }
}