using Runtime.Infrastructure.AssetProvider;
using Runtime.Infrastructure.DOTweenAnimationServices.Score;
using Runtime.Infrastructure.EntryPoint;
using Runtime.Infrastructure.Factories;
using Runtime.Infrastructure.UserData;
using Runtime.UI.Screens;
using UnityEngine;
using Zenject;

namespace Runtime.Infrastructure.Bootstrap
{
    public sealed class ProjectInstaller : MonoInstaller, IInitializable
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<ProjectInstaller>().FromInstance(this).AsSingle();
            
            Container.Bind<UserData.UserData>().AsSingle();
            Container.Bind<ScreenContainer>().AsSingle();
            Container.Bind<IScoreAnimationService>().To<ScoreAnimationService>().AsSingle();

            Container.BindInterfacesAndSelfTo<UserDataSaveLoadService>().AsSingle();
            Container.BindInterfacesAndSelfTo<ResourcesAssetProvider>().AsSingle();
            Container.BindInterfacesAndSelfTo<AsyncSceneLoader>().AsSingle();
            Container.BindInterfacesAndSelfTo<EntryPoint.EntryPoint>().AsSingle();
            Container.BindInterfacesAndSelfTo<UIFactory>().AsSingle();
        }

        public void Initialize()
        {
            ProjectInitializer projectInitializer = Container.Instantiate<ProjectInitializer>();
            Container.Bind<ProjectInitializer>().FromInstance(projectInitializer).AsSingle();
            
            projectInitializer.Initialize();
        }
    }
}
