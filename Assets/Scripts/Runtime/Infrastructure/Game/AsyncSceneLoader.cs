using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Runtime.Infrastructure.Game
{
    public sealed class AsyncSceneLoader : ISceneLoader
    {
        public async void LoadScene(string sceneName, Action sceneLoaded = null)
        {
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);

            while (!asyncOperation.isDone)
            {
                await UniTask.DelayFrame(1);
            }
            
            sceneLoaded?.Invoke();
        }
    }
}