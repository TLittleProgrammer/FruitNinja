using UnityEngine;

namespace Runtime.SlicableObjects.Spawner
{
    public struct SlicableObjectSpawnerData
    {
        private Vector2 _firstSpawnPoint;
        private Vector2 _secondSpawnPoint;

        private float _timeOffset;
        private int _packSize;

        public SlicableObjectSpawnerData(Vector2 firstSpawnPoint, Vector2 secondSpawnPoint, float timeOffset, int packSize)
        {
            _firstSpawnPoint = firstSpawnPoint;
            _secondSpawnPoint = secondSpawnPoint;
            _timeOffset = timeOffset;
            _packSize = packSize;
        }

        public Vector2 FirstSpawnPoint  => _firstSpawnPoint;
        public Vector2 SecondSpawnPoint => _secondSpawnPoint;
        
        public float TimeOffset => _timeOffset;
        public int PackSize     => _packSize;
    }
}