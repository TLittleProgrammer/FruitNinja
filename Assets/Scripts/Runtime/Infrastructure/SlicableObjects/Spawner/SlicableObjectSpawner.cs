using System;
using Runtime.Infrastructure.Data;

namespace Runtime.Infrastructure.SlicableObjects.Spawner
{
    [Serializable]
    public class SlicableObjectSpawnerData
    {
        //TODO поправить названия переменных
        public int Weight;
        public MinMaxValue FirstSpawnPoint;
        public MinMaxValue SecondSpawnPoint;
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
            MinMaxValue xPositions,
            MinMaxValue yPositions,
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
            FirstSpawnPoint     = xPositions;
            SecondSpawnPoint    = yPositions;
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
        }
    }
}