using System;

namespace Runtime.Infrastructure.EntryPoint
{
    public interface ISceneLoader
    {
        void LoadScene(string sceneName, Action sceneLoaded = null);
    }
}