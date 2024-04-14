using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Runtime.Infrastructure.NotStateMachine
{
    public sealed class AsyncSceneLoader : ISceneLoader
    {
        public async void LoadScene(string sceneName, Action sceneLoaded = null)
        {
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);

            await UniTask.WaitWhile(() => !asyncOperation.isDone);

            sceneLoaded?.Invoke();
        }
    }
}