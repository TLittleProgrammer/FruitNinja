using Runtime.UI.Screens;

namespace Runtime.Infrastructure.Bootstrap
{
    public sealed class MainMenuInitializer : AbstractIntializer
    {
        private void Awake()
        {
            UiFactory.LoadScreen<MainMenu>(ScreenType.MainMenu, SceneCanvasTransform);
            
            GameStateMachine.HideLoadingScreen();
        }
    }
}