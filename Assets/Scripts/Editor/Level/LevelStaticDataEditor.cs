using Runtime.Infrastructure.SlicableObjects.Spawner;
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
                        .Add(new(
                            marker.SideType,
                            marker.FirstSpawnPointPercent,
                            marker.SecondSpawnPointPercent,
                            marker.MainDirectionOffset,
                            marker.FirstOffsetAngle,
                            marker.SecondOffsetAngle,
                            marker.PackSize,
                            marker.Weight,
                            marker.VelocityXMin,
                            marker.VelocityXMax,
                            marker.VelocityYMin,
                            marker.VelocityYMax,
                            marker.SpawnPackOffsetMin,
                            marker.SpawnPackOffsetMax
                            ));
                }
            }

            EditorUtility.SetDirty(target);
        }
    }
}