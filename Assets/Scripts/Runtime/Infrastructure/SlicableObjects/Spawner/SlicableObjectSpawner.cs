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
        public MinMaxValue XVelocity;
        public MinMaxValue YVelocity;
        public MinMaxValue SpawnOffset;
        public float MainDirectionOffset;
        public float FirstOffset;
        public float SecondOffset;

        public int PackSize;

        public SlicableObjectSpawnerData(
            MinMaxValue xPositions,
            MinMaxValue yPositions,
            MinMaxValue xVelocity,
            MinMaxValue yVelocity,
            MinMaxValue spawnOffset,
            float mainDirectionOffset,
            float firstOffset,
            float secondOffset,
            int packSize,
            int weight
            )
        {
            FirstSpawnPoint     = xPositions;
            SecondSpawnPoint    = yPositions;
            XVelocity           = xVelocity;
            YVelocity           = yVelocity;
            SpawnOffset         = spawnOffset;
            MainDirectionOffset = mainDirectionOffset;
            FirstOffset         = firstOffset;
            SecondOffset        = secondOffset;
            PackSize            = packSize;
            Weight              = weight;
        }
    }
}