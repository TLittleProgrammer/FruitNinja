using System.Collections.Generic;

namespace ObjectPool.Runtime.ObjectPool
{
    public sealed class QueueObjectPool<TElement> : IObjectPool<TElement>
    {
        private Queue<TElement> _queue;

        public QueueObjectPool()
        {
            _queue = new();
            Size = 0;
        }

        public int Size { get; private set; }

        public void Set(TElement element)
        {
            _queue.Enqueue(element);

            Size++;
        }

        public TElement Get()
        {
            Size--;
            
            return _queue.Dequeue();
        }
    }
}