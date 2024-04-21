using System.Collections.Generic;

namespace Runtime.Infrastructure.SlicableObjects
{
    public interface ISlicableObjectCounterOnMap
    {
        void AddType(SlicableObjectType slicableObjectType);
        void RemoveType(SlicableObjectType slicableObjectType);
        int GetCountByType(SlicableObjectType slicableObjectType);
    }
    
    public sealed class SlicableObjectCounterOnMap : ISlicableObjectCounterOnMap
    {
        private Dictionary<SlicableObjectType, int> _counter;

        public SlicableObjectCounterOnMap()
        {
            _counter = new();
        }
        
        public void AddType(SlicableObjectType slicableObjectType)
        {
            if (_counter.TryGetValue(slicableObjectType, out int count))
            {
                _counter[slicableObjectType]++;
                return;
            }

            _counter.Add(slicableObjectType, 1);
        }

        public void RemoveType(SlicableObjectType slicableObjectType)
        {
            if (_counter.TryGetValue(slicableObjectType, out int count))
            {
                _counter[slicableObjectType]--;
            }
        }

        public int GetCountByType(SlicableObjectType slicableObjectType)
        {
            return _counter.TryGetValue(slicableObjectType, out int count) ? count : 0;
        }
    }
}