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
    
    public interface IAsyncInitializable<TPayloadFirst, TPayloadSecond>
    {
        UniTask AsyncInitialize(TPayloadFirst gameStateMachine, TPayloadSecond mimikService);
    }
}