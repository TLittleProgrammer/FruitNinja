namespace Runtime.Infrastructure.Game
{
    public interface IGameStateMachine : IAsyncInitializable
    {
        void AsyncLoadScene(string sceneName);
        void HideLoadingScreen();
    }
}