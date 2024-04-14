namespace Runtime.Infrastructure.NotStateMachine
{
    public interface IGameStateMachine : IAsyncInitializable
    {
        void AsyncLoadScene(string sceneName);
        void HideLoadingScreen();
    }
}