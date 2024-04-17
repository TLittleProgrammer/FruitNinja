using Runtime.Infrastructure.Data;
using UnityEngine;

namespace Runtime.Infrastructure.SlicableObjects.Spawner
{
    public class SlicableObjectSpawnerMarker : MonoBehaviour
    {
        [Space(5)]
        [Header("Вес")]
        [Min(1)]
        public int Weight = 1;

        [Header("Точки спавна")]
        public MinMaxValue XPositions;
        public MinMaxValue YPositions;

        [Space(5)]
        [Header("Скорость")]
        [Min(0f)]
        public float VelocityXMin;
        [Min(0f)]
        public float VelocityXMax;
        [Min(0f)]
        public float VelocityYMin;
        [Min(0f)]
        public float VelocityYMax;

        [Header("Задержка между спавнами объектов пака")]
        [Min(0f)]
        public float SpawnPackOffsetMin;
        [Min(0f)]
        public float SpawnPackOffsetMax;

        
        [Space(5)]
        [Header("Смещение угла")]
        public float MainDirectionOffset;
        public float FirstOffsetAngle;
        public float SecondOffsetAngle;
        
        [Space(5)]
        [Header("Размер пака")]
        public int PackSize;

        private void OnValidate()
        {
            if (PackSize <= 0)
            {
                PackSize = 1;
            }
        }
    }
}