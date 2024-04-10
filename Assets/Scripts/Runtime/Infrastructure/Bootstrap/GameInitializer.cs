using Runtime.SlicableObjects;
using Runtime.UI.Screens;
using Zenject;

namespace Runtime.Infrastructure.Bootstrap
{
    public sealed class GameInitializer : AbstractIntializer
    {
        [Inject] private SlicableSpriteContainer _slicableSpriteContainer;
        
        private async void Awake()
        {
            await _slicableSpriteContainer.AsyncInitialize();
            
            UiFactory.LoadScreen<GameScreen>(ScreenType.Game, SceneCanvasTransform);
            
            GameStateMachine.HideLoadingScreen();
        }
    }
}