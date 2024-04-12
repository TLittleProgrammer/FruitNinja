namespace ObjectPool.Runtime.ObjectPool
{
    public interface IObjectPool<TElement>
    {
        int Size { get; }
        
        void Set(TElement element);
        TElement Get();
    }
}