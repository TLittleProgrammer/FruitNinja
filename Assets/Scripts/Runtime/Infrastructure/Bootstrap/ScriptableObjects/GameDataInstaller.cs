using System;
using Runtime.StaticData.Level;
using UnityEngine;
using Zenject;

namespace Runtime.Infrastructure.Bootstrap.ScriptableObjects
{
    [CreateAssetMenu(fileName = "GameSpritesInstaller", menuName = "Installers/GameSpritesInstaller")]
    public class GameDataInstaller : ScriptableObjectInstaller<GameDataInstaller>
    {
        public SlicableSpriteProvider SlicableSpriteProvider;
        public LevelStaticData LevelStaticData;
        public PoolSettings PoolSettings;
        
        public override void InstallBindings()
        {
            Container.Bind<SlicableSpriteProvider>().FromInstance(SlicableSpriteProvider).AsSingle();
            Container.Bind<LevelStaticData>().FromInstance(LevelStaticData).AsSingle();
            Container.Bind<PoolSettings>().FromInstance(PoolSettings).AsSingle();
            
        }
    }

    [Serializable]
    public class PoolSettings
    {
        public int PoolInitialSize;
    }
}