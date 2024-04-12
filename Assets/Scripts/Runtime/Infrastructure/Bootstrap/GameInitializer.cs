using System;
using Cysharp.Threading.Tasks;
using ObjectPool.Runtime.ObjectPool;
using Runtime.Infrastructure.Bootstrap.ScriptableObjects;
using Runtime.Infrastructure.Factories;
using Runtime.SlicableObjects;
using Runtime.UI.Screens;
using UnityEngine;
using Zenject;

namespace Runtime.Infrastructure.Bootstrap
{
    public sealed class GameInitializer : AbstractIntializer
    {
        [SerializeField] private Transform _slicableViewsParent;
        
        [Inject] private SlicableSpriteContainer _slicableSpriteContainer;
        [Inject] private QueueObjectPool<SlicableObjectView> _queueObjectPool;
        [Inject] private IWorldFactory _worldFactory;
        [Inject] private PoolSettings _poolSettings;
        [Inject] private GameScreenPositionResolver _gameScreenPositionResolver;
        
        private async void Awake()
        {
            await _slicableSpriteContainer.AsyncInitialize();
            await _gameScreenPositionResolver.AsyncInitialize();
            await InitializePool();

            UiFactory.LoadScreen<GameScreen>(ScreenType.Game, SceneCanvasTransform);
            
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