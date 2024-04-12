using System;

namespace Runtime.SlicableObjects.Spawner
{
    [Serializable]
    public struct SlicableObjectSpawnerData
    {
        public SideType SideType;

        public int Weight;
        public float FirstSpawnPoint;
        public float SecondSpawnPoint;
        public float MainDirectionOffset;
        public float FirstOffset;
        public float SecondOffset;

        public int PackSize;

        public SlicableObjectSpawnerData(SideType sideType, float firstSpawnPoint, float secondSpawnPoint, float mainDirectionOffset, float firstOffset, float secondOffset, int packSize, int weight)
        {
            SideType            = sideType;
            FirstSpawnPoint     = firstSpawnPoint;
            SecondSpawnPoint    = secondSpawnPoint;
            MainDirectionOffset = mainDirectionOffset;
            FirstOffset         = firstOffset;
            SecondOffset        = secondOffset;
            PackSize            = packSize;
            Weight = weight;

            if (FirstSpawnPoint > SecondSpawnPoint)
            {
                (FirstSpawnPoint, SecondSpawnPoint) = (SecondSpawnPoint, FirstSpawnPoint);
            }
        }
    }
}