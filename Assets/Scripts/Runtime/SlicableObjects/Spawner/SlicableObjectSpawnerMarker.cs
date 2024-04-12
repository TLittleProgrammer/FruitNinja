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
        
        [Header("Смещение ")]
        [Range(0f, 1f)]
        public float FirstSpawnPointPercent;
        [Range(0f, 1f)]
        public float SecondSpawnPointPercent;
        
        [Space(5)]
        [Header("Смещение")]
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