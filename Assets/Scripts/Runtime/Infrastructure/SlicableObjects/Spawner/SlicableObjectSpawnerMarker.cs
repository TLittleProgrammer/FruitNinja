using UnityEngine;

namespace Runtime.Infrastructure.SlicableObjects.Spawner
{
    public class SlicableObjectSpawnerMarker : MonoBehaviour
    {
        public SideType SideType = SideType.None;

        [Space(5)]
        [Header("Вес")]
        [Min(1)]
        public int Weight = 1;
        
        [Header("Смещение точек спавна")]
        [Range(0f, 1f)]
        public float FirstSpawnPointPercent;
        [Range(0f, 1f)]
        public float SecondSpawnPointPercent;

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