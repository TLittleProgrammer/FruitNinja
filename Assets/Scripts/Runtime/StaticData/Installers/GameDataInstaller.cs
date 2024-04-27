using Runtime.Infrastructure.Combo;
using Runtime.StaticData.Animations;
using Runtime.StaticData.Boosts;
using Runtime.StaticData.Level;
using Runtime.StaticData.UI;
using UnityEngine;
using Zenject;

namespace Runtime.StaticData.Installers
{
    [CreateAssetMenu(fileName = "GameSpritesInstaller", menuName = "Installers/GameSpritesInstaller")]
    public sealed class GameDataInstaller : ScriptableObjectInstaller<GameDataInstaller>
    {
        public BombSettings BombSettings;
        public SlicableSpriteProvider SlicableSpriteProvider;
        public LevelStaticData LevelStaticData;
        public PoolSettings PoolSettings;
        public BlotEffectSettings BlotEffectSettings;
        public FlyingHealthViewStaticData FlyingHealthViewStaticData;
        
        public override void InstallBindings()
        {
            Container.Bind<SlicableSpriteProvider>().FromInstance(SlicableSpriteProvider).AsSingle();
            Container.Bind<LevelStaticData>().FromInstance(LevelStaticData).AsSingle();
            Container.Bind<ComboData>().FromInstance(LevelStaticData.ComboData).AsSingle();
            Container.Bind<PoolSettings>().FromInstance(PoolSettings).AsSingle();
            Container.Bind<BlotEffectSettings>().FromInstance(BlotEffectSettings).AsSingle();
            Container.Bind<FlyingHealthViewStaticData>().FromInstance(FlyingHealthViewStaticData).AsSingle();
            Container.Bind<BombSettings>().FromInstance(BombSettings).AsSingle();
        }
    }
}