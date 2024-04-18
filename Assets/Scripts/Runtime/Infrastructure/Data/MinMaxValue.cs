using System;
using Random = UnityEngine.Random;

namespace Runtime.Infrastructure.Data
{
    [Serializable]
    public struct MinMaxValue
    {
        public float Min;
        public float Max;

        public MinMaxValue(float max, float min)
        {
            Max = max;
            Min = min;

            if (Min > Max)
            {
                (Min, Max) = (Max, Min);
            }
        }

        public float GetRandomValue()
        {
            return Random.Range(Min, Max);
        }
    }
}