using UnityEngine;

namespace Runtime.Infrastructure.SlicableObjects.Spawner
{
    public class SlicableObjectSpawnerMarker : MonoBehaviour
    {
        public SlicableObjectSpawnerData SpawnerData;

        private void OnValidate()
        {
            if (SpawnerData.PackSize <= 0)
            {
                SpawnerData.PackSize = 1;
            }
        }
    }
}