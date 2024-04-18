namespace Runtime.Infrastructure.NotStateMachine
{
    public interface IEntryPoint : IAsyncInitializable
    {
        void AsyncLoadScene(string sceneName);
        void HideLoadingScreen();
    }
}