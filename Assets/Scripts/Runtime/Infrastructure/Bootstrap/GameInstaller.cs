﻿using ObjectPool.Runtime.ObjectPool;
using Runtime.Infrastructure.Factories;
using Runtime.SlicableObjects;
using Runtime.SlicableObjects.Spawner;
using Zenject;

namespace Runtime.Infrastructure.Bootstrap
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<SlicableSpriteContainer>().AsSingle();
            Container.Bind<GameScreenPositionResolver>().AsSingle();

            Container.BindInterfacesAndSelfTo<WorldFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<QueueObjectPool<SlicableObjectView>>().AsSingle();
            Container.BindInterfacesAndSelfTo<SlicableObjectSpawnerManager>().AsSingle();
        }
    }
}