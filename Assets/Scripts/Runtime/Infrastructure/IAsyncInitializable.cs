using Cysharp.Threading.Tasks;

namespace Runtime.Infrastructure
{
    public interface IAsyncInitializable
    {
        UniTask AsyncInitialize();
    }
    
    public interface IAsyncInitializable<TPayload>
    {
        UniTask AsyncInitialize(TPayload payload);
    }
}