using Runtime.Infrastructure.Factories;
using Runtime.Infrastructure.Game;
using Runtime.Infrastructure.NotStateMachine;
using UnityEngine;
using Zenject;

namespace Runtime.Infrastructure.Bootstrap
{
    public abstract class AbstractIntializer : MonoBehaviour
    {
        [SerializeField] protected Transform SceneCanvasTransform;
        
        [Inject] protected IGameStateMachine GameStateMachine;
        [Inject] protected IUIFactory UiFactory;
    }
}