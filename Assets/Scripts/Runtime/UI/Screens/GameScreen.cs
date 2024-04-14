using Cysharp.Threading.Tasks;
using Runtime.Infrastructure;
using Runtime.Infrastructure.Factories;
using Runtime.Infrastructure.Game;
using Runtime.UI.Game;
using UnityEngine;
using Zenject;

namespace Runtime.UI.Screens
{
    public sealed class GameScreen : MonoBehaviour, IAsyncInitializable
    {
        [SerializeField] private HeartsView _heartsView;
        
        private IUIFactory _uiFactory;

        [Inject]
        private void Construct(IUIFactory uiFactory)
        {
            _uiFactory = uiFactory;
        }
        
        public async UniTask AsyncInitialize()
        {
            await _heartsView.AsyncInitialize(_uiFactory);
        }
    }
}