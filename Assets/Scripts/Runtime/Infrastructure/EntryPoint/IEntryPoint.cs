namespace Runtime.Infrastructure.EntryPoint
{
    public interface IEntryPoint : IAsyncInitializable
    {
        void AsyncLoadScene(string sceneName);
        void HideLoadingScreen();
    }
}