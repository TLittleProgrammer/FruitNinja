using UnityEngine;
using Zenject;

namespace Runtime.Infrastructure.Bootstrap.ScriptableObjects
{
    [CreateAssetMenu(fileName = "ProjectSettingsInstaller", menuName = "Installers/ProjectSettingsInstaller")]
    public class ProjectSettingsInstaller : ScriptableObjectInstaller<ProjectSettingsInstaller>
    {
        public ButtonAnimationSettings ButtonAnimationSettings;
        
        public override void InstallBindings()
        {
            Container.Bind<ButtonAnimationSettings>().FromInstance(ButtonAnimationSettings).AsSingle();
        }
    }
}