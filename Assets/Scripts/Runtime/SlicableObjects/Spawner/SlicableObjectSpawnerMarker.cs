using System;
using UnityEngine;

namespace Runtime.SlicableObjects.Spawner
{
    public class SlicableObjectSpawnerMarker : MonoBehaviour
    {
        public SideType SideType = SideType.None;
        
        [Range(0f, 1f)]
        public float FirstSpawnPointPercent;
        [Range(0f, 1f)]
        public float SecondSpawnPointPercent;

        public float TimeOffset;
        public float OffsetAngle;
        public int PackSize;


        private void OnValidate()
        {
            if (PackSize <= 0)
            {
                PackSize = 1;
            }

            if (TimeOffset <= 0)
            {
                TimeOffset = 3f;
            }

            if (OffsetAngle <= 0)
            {
                OffsetAngle = 30f;
            }
        }
    }
}