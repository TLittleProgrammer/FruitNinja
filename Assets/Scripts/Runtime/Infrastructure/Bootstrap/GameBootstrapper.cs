using Runtime.Constants;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Runtime.Infrastructure.Bootstrap
{
    public class GameBootstrapper : MonoBehaviour
    {
        private void Awake()
        {
            ProjectContext projectContext = FindObjectOfType<ProjectContext>();

            if (projectContext is null)
            {
                SceneManager.LoadScene(SceneNames.Bootstrap);
            }
        }
    }
}