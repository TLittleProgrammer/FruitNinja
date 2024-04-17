using UnityEngine;
using Zenject;

namespace Runtime.StaticData.Installers
{
    [CreateAssetMenu(fileName = "ProjectSettings", menuName = "Installers/ProjectSettings")]
    public class ProjectSettings : ScriptableObjectInstaller
    {
        public int ApplicationTargetFrameCount;
        public int QualitySettingsVSyncCount;
    }
}