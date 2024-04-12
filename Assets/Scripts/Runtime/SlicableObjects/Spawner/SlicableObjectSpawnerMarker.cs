using UnityEngine;

namespace Runtime.SlicableObjects.Spawner
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
        [Header("Скорости")]
        [Min(1f)]
        public float SpeedXMin;
        [Min(1f)]
        public float SpeedXMax;
        [Min(1f)]
        public float SpeedYMin;
        [Min(1f)]
        public float SpeedYMax;
        
        [Space(5)]
        [Header("Смещение угла")]
        public float MainDirectionOffset;
        public float FirstOffset;
        public float SecondOffset;
        
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