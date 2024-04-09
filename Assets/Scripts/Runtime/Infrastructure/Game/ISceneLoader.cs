using System;

namespace Runtime.Infrastructure.Game
{
    public interface ISceneLoader
    {
        void LoadScene(string sceneName, Action sceneLoaded = null);
    }
}