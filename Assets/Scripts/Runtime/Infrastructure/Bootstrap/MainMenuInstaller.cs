using UnityEngine;
using Zenject;

namespace Runtime.Infrastructure.Bootstrap
{
    public class MainMenuInstaller : MonoInstaller, IInitializable
    {
        [SerializeField] private Transform _sceneCanvasTransform;

        public override void InstallBindings()
        {
            Container.BindInterfacesTo<MainMenuInstaller>().FromInstance(this).AsSingle();
            
            Container.Bind<MainMenuInitializer>().AsSingle().WithArguments(_sceneCanvasTransform);
        }

        public void Initialize()
        {
            Container.Resolve<MainMenuInitializer>().Initialize();
        }
    }
}