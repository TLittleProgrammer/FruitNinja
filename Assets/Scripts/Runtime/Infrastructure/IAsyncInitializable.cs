using Cysharp.Threading.Tasks;

namespace Runtime.Infrastructure
{
    public interface IAsyncInitializable
    {
        UniTask AsyncInitialize();
    }
}