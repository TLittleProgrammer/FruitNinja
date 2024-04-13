using Runtime.StaticData.Animations;
using Runtime.StaticData.UI;
using UnityEngine;
using Zenject;

namespace Runtime.StaticData.Installers
{
    [CreateAssetMenu(fileName = "ProjectSettingsInstaller", menuName = "Installers/ProjectSettingsInstaller")]
    public sealed class ProjectSettingsInstaller : ScriptableObjectInstaller<ProjectSettingsInstaller>
    {
        public ButtonAnimationSettings ButtonAnimationSettings;
        public LoadingScreenFadeDuration LoadingScreenFadeDuration;
        
        public override void InstallBindings()
        {
            Container.Bind<ButtonAnimationSettings>().FromInstance(ButtonAnimationSettings).AsSingle();
            Container.Bind<LoadingScreenFadeDuration>().FromInstance(LoadingScreenFadeDuration).AsSingle();
        }
    }
}