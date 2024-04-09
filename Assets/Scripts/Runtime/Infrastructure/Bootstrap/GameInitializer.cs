using Runtime.UI.Screens;

namespace Runtime.Infrastructure.Bootstrap
{
    public sealed class GameInitializer : AbstractIntializer
    {
        private void Awake()
        {
            UiFactory.LoadScreen<GameScreen>(ScreenType.Game, SceneCanvasTransform);
            
            GameStateMachine.HideLoadingScreen();
        }
    }
}