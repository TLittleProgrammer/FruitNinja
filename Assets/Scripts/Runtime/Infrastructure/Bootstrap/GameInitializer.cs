using Cysharp.Threading.Tasks;
using ObjectPool.Runtime.ObjectPool;
using Runtime.Infrastructure.Factories;
using Runtime.SlicableObjects;
using Runtime.StaticData.Installers;
using Runtime.UI.Screens;
using UnityEngine;
using Zenject;

namespace Runtime.Infrastructure.Bootstrap
{
    public sealed class GameInitializer : AbstractIntializer
    {
        private const string PathToGameCameraPrefab = "Prefabs/GameCamera";
        
        [SerializeField] private Transform _slicableViewsParent;
        [SerializeField] private Canvas _gameCanvas;
        
        [Inject] private SlicableSpriteContainer _slicableSpriteContainer;
        [Inject] private QueueObjectPool<SlicableObjectView> _queueObjectPool;
        [Inject] private IWorldFactory _worldFactory;
        [Inject] private PoolSettings _poolSettings;
        [Inject] private GameScreenPositionResolver _gameScreenPositionResolver;
        
        private async void Awake()
        {
            await _slicableSpriteContainer.AsyncInitialize();
            await InitializePool();

            Camera camera = await UiFactory.LoadUIObjectByPath<Camera>(PathToGameCameraPrefab, null, Vector3.back * 10);
            _gameCanvas.worldCamera = camera;
            
            UiFactory.LoadScreen<GameScreen>(ScreenType.Game, SceneCanvasTransform);
            await _gameScreenPositionResolver.AsyncInitialize(camera);
            
            GameStateMachine.HideLoadingScreen();
        }

        private async UniTask InitializePool()
        {
            for (int i = 0; i < _poolSettings.PoolInitialSize; i++)
            {
                SlicableObjectView slicableObjectView = await _worldFactory.CreateSlicableObjectView(_slicableViewsParent);
                _queueObjectPool.Set(slicableObjectView);
            }

            await UniTask.CompletedTask;
        }
    }
}