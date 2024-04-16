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
        }

        public void Initialize()
        {
            MainMenuInitializer menuInitializer = Container.Instantiate<MainMenuInitializer>(new[] { _sceneCanvasTransform });
            
            menuInitializer.Initialize();
        }
    }
}