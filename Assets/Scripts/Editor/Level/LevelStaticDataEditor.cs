using Runtime.SlicableObjects.Spawner;
using Runtime.StaticData.Level;
using UnityEditor;
using UnityEngine;

namespace Editor.Level
{
    [CustomEditor(typeof(LevelStaticData))]
    public class LevelStaticDataEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            LevelStaticData levelStaticData = (LevelStaticData)target;
            
            if (GUILayout.Button($"Collect datas"))
            {
                SlicableObjectSpawnerMarker[] markers = FindObjectsOfType<SlicableObjectSpawnerMarker>(true);

                levelStaticData.SlicableObjectSpawnerDataList.Clear();

                foreach (SlicableObjectSpawnerMarker marker in markers)
                {
                    levelStaticData
                        .SlicableObjectSpawnerDataList
                        .Add(new(marker.FirstSpawnPointPercent, marker.SecondSpawnPointPercent, marker.OffsetAngle, marker.PackSize, marker.SideType));
                }
            }

            EditorUtility.SetDirty(target);
        }
    }
}