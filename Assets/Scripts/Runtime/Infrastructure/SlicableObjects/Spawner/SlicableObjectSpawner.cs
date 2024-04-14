using System;

namespace Runtime.Infrastructure.SlicableObjects.Spawner
{
    [Serializable]
    public class SlicableObjectSpawnerData
    {
        public SideType SideType;

        //TODO поправить названия переменных
        public int Weight;
        public float FirstSpawnPoint;
        public float SecondSpawnPoint;
        public float MainDirectionOffset;
        public float FirstOffset;
        public float SecondOffset;
        public float VelocityXMin;
        public float VelocityXMax;
        public float VelocityYMin;
        public float VelocityYMax;
        public float PackSpawnOffsetMin;
        public float PackSpawnOffsetMax;

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
            float velocityXMin,
            float velocityXMax,
            float velocityYMin,
            float velocityYMax,
            float packSpawnOffsetMin,
            float packSpawnOffsetMax
            )
        {
            SideType            = sideType;
            FirstSpawnPoint     = firstSpawnPoint;
            SecondSpawnPoint    = secondSpawnPoint;
            MainDirectionOffset = mainDirectionOffset;
            FirstOffset         = firstOffset;
            SecondOffset        = secondOffset;
            PackSize            = packSize;
            Weight              = weight;
            VelocityXMin        = velocityXMin;
            VelocityXMax        = velocityXMax;
            VelocityYMin        = velocityYMin;
            VelocityYMax        = velocityYMax;
            PackSpawnOffsetMin  = packSpawnOffsetMin;
            PackSpawnOffsetMax  = packSpawnOffsetMax;

            if (FirstSpawnPoint > SecondSpawnPoint)
            {
                (FirstSpawnPoint, SecondSpawnPoint) = (SecondSpawnPoint, FirstSpawnPoint);
            }
        }
    }
}