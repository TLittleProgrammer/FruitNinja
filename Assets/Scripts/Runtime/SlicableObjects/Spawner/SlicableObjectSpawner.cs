using System;

namespace Runtime.SlicableObjects.Spawner
{
    [Serializable]
    public struct SlicableObjectSpawnerData
    {
        public SideType SideType;
        
        public float FirstSpawnPoint;
        public float SecondSpawnPoint;

        public float TimeOffset;
        public int PackSize;

        public SlicableObjectSpawnerData(float firstSpawnPoint, float secondSpawnPoint, float timeOffset, int packSize, SideType sideType)
        {
            FirstSpawnPoint = firstSpawnPoint;
            SecondSpawnPoint = secondSpawnPoint;
            TimeOffset = timeOffset;
            PackSize = packSize;
            SideType = sideType;

            if (FirstSpawnPoint > SecondSpawnPoint)
            {
                (FirstSpawnPoint, SecondSpawnPoint) = (SecondSpawnPoint, FirstSpawnPoint);
            }
        }
    }
}