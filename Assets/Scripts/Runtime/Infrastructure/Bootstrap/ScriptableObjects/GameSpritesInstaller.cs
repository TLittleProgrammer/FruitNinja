using UnityEngine;
using Zenject;

namespace Runtime.Infrastructure.Bootstrap.ScriptableObjects
{
    [CreateAssetMenu(fileName = "GameSpritesInstaller", menuName = "Installers/GameSpritesInstaller")]
    public class GameSpritesInstaller : ScriptableObjectInstaller<GameSpritesInstaller>
    {
        public SlicableSpriteProvider SlicableSpriteProvider;
        
        public override void InstallBindings()
        {
            Container.Bind<SlicableSpriteProvider>().FromInstance(SlicableSpriteProvider).AsSingle();
        }
    }
}