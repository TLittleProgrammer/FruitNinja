using Runtime.Infrastructure.Factories;
using Runtime.SlicableObjects;
using Zenject;

namespace Runtime.Infrastructure.Bootstrap
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<SlicableSpriteContainer>().AsSingle();

            Container.BindInterfacesAndSelfTo<WorldFactory>().AsSingle();
            // ? Container.BindInterfacesAndSelfTo<QueueObjectPool<SlicableObjectView>>().AsSingle();
        }
    }
}