using Runtime.Infrastructure.AssetProvider;
using Runtime.Infrastructure.Factories;
using Runtime.Infrastructure.NotStateMachine;
using Runtime.UI.Screens;
using Zenject;

namespace Runtime.Infrastructure.Bootstrap
{
    public sealed class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<UserData.UserData>().AsSingle();
            Container.Bind<UserData.UserDataSaveLoadService>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<ResourcesAssetProvider>().AsSingle();
            Container.BindInterfacesAndSelfTo<AsyncSceneLoader>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameStateMachine>().AsSingle();

            Container.Bind<ScreenContainer>().AsSingle();
            
            Container.Bind<IUIFactory>().To<UIFactory>().AsSingle();
        }
    }
}
