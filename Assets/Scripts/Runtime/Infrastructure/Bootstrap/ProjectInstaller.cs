using Runtime.Infrastructure.AssetProvider;
using Runtime.Infrastructure.Factories;
using Runtime.Infrastructure.Game;
using Runtime.UI.Screens;
using Zenject;

namespace Runtime.Infrastructure.Bootstrap
{
    public sealed class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<ResourcesAssetProvider>().AsSingle();
            Container.BindInterfacesAndSelfTo<AsyncSceneLoader>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameStateMachine>().AsSingle();

            Container.Bind<ScreenContainer>().AsSingle();
            
            Container.Bind<IUIFactory>().To<UIFactory>().AsSingle();
        }
    }
}
