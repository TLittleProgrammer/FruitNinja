using Runtime.Infrastructure;
using UnityEngine;

namespace Runtime.SlicableObjects.Spawner
{
    public class SlicableObjectSpawnerService : ITickable
    {
        private readonly SlicableObjectSpawnerData _slicableObjectSpawnerData;

        private float _currentTime;
        
        public SlicableObjectSpawnerService(SlicableObjectSpawnerData slicableObjectSpawnerData)
        {
            _slicableObjectSpawnerData = slicableObjectSpawnerData;
            _currentTime = 0f;
        }

        private float TargetTime => _slicableObjectSpawnerData.TimeOffset;

        public void Tick()
        {
            _currentTime += Time.deltaTime;

            if (_currentTime <= TargetTime)
            {
                _currentTime = 0f;
            }
        }
    }
}