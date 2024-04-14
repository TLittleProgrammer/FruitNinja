using System;

namespace Runtime.Infrastructure.NotStateMachine
{
    public interface ISceneLoader
    {
        void LoadScene(string sceneName, Action sceneLoaded = null);
    }
}