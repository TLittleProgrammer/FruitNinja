using Runtime.Infrastructure.Effects;
using Runtime.StaticData.Animations;
using Runtime.StaticData.Level;
using Runtime.StaticData.UI;
using UnityEngine;
using Zenject;

namespace Runtime.StaticData.Installers
{
    [CreateAssetMenu(fileName = "GameSpritesInstaller", menuName = "Installers/GameSpritesInstaller")]
    public sealed class GameDataInstaller : ScriptableObjectInstaller<GameDataInstaller>
    {
        public SlicableSpriteProvider SlicableSpriteProvider;
        public LevelStaticData LevelStaticData;
        public PoolSettings PoolSettings;
        public BlotEffectSettings BlotEffectSettings;
        
        public override void InstallBindings()
        {
            Container.Bind<SlicableSpriteProvider>().FromInstance(SlicableSpriteProvider).AsSingle();
            Container.Bind<LevelStaticData>().FromInstance(LevelStaticData).AsSingle();
            Container.Bind<PoolSettings>().FromInstance(PoolSettings).AsSingle();
            Container.Bind<BlotEffectSettings>().FromInstance(BlotEffectSettings).AsSingle();
        }
    }
}