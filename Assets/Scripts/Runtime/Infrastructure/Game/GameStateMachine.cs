using Cysharp.Threading.Tasks;
using Runtime.Infrastructure.Factories;
using Runtime.UI.Screens;
using Zenject;
using Object = UnityEngine.Object;

namespace Runtime.Infrastructure.Game
{
    public sealed class GameStateMachine : IGameStateMachine
    {
        private const string PathToRootUI = "Prefabs/UI/RootUI";
        
        private readonly IUIFactory _uiFactory;
        private readonly ISceneLoader _sceneLoader;
        private readonly DiContainer _diContainer;

        private LoadingScreen _loadingScreen;
        private RootUI _rootUI;

        public GameStateMachine(IUIFactory uiFactory, ISceneLoader sceneLoader, DiContainer diContainer)
        {
            _uiFactory = uiFactory;
            _sceneLoader = sceneLoader;
            _diContainer = diContainer;
        }

        public async UniTask AsyncInitialize()
        {
            await CreateRootUI();
            
            CreateLoadingScreen();
        }

        public async void AsyncLoadScene(string sceneName)
        {
            await _loadingScreen.Show();
            
            _uiFactory.Dispose();
            
            _sceneLoader.LoadScene(sceneName);
        }

        public void HideLoadingScreen()
        {
            _loadingScreen.Hide();
        }

        private async UniTask CreateRootUI()
        {
            _rootUI = await _uiFactory.LoadUIObjectByPath<RootUI>(PathToRootUI, null);

            _diContainer.Bind<RootUI>().FromInstance(_rootUI).AsSingle();
            
            Object.DontDestroyOnLoad(_rootUI);
        }

        private void CreateLoadingScreen()
        {
            _loadingScreen = _uiFactory.LoadScreen<LoadingScreen>(ScreenType.Loading, _rootUI.CanvasTransform);
        }
    }
}