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
        public float SpeedXMin;
        public float SpeedXMax;
        public float SpeedYMin;
        public float SpeedYMax;

        public int PackSize;

        public SlicableObjectSpawnerData(
            SideType sideType,
            float firstSpawnPoint,
            float secondSpawnPoint,
            float mainDirectionOffset,
            float firstOffset,
            float secondOffset,
            int packSize,
            int weight,
            float speedXMin,
            float speedXMax,
            float speedYMin,
            float speedYMax
            )
        {
            SideType            = sideType;
            FirstSpawnPoint     = firstSpawnPoint;
            SecondSpawnPoint    = secondSpawnPoint;
            MainDirectionOffset = mainDirectionOffset;
            FirstOffset         = firstOffset;
            SecondOffset        = secondOffset;
            PackSize            = packSize;
            Weight = weight;
            SpeedXMin = speedXMin;
            SpeedXMax = speedXMax;
            SpeedYMin = speedYMin;
            SpeedYMax = speedYMax;

            if (FirstSpawnPoint > SecondSpawnPoint)
            {
                (FirstSpawnPoint, SecondSpawnPoint) = (SecondSpawnPoint, FirstSpawnPoint);
            }
        }
    }
}