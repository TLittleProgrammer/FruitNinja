using Runtime.Extensions;
using Runtime.Infrastructure.SlicableObjects.Spawner;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace Editor.SlicableObjectSpawner
{
    [CustomEditor(typeof(SlicableObjectSpawnerMarker))]
    public class SlicableObjectSpawnerMarkerEditor : UnityEditor.Editor
    {
        private static float OrthographicSize;
        private static float Resolution;
        private static float HorizontalSize;
        private static float HorizontalPlusOneStepSize;
        private static float SizeSum;
        private Camera _camera;
        
        private void OnEnable()
        {
            _camera = Camera.main;
            
            if (_camera is null)
            {
                Camera cameraPrefab = Resources.Load<Camera>("Prefabs/GameCamera");
                _camera = Instantiate(cameraPrefab, new Vector3(0f, 0f, -10f), Quaternion.identity, null);
            }

            OrthographicSize            = _camera.orthographicSize;
            Resolution                  = (float)Screen.width / Screen.height;
            HorizontalSize              = Resolution * OrthographicSize;
            HorizontalPlusOneStepSize   = Resolution * (OrthographicSize + 1);

            SizeSum = HorizontalPlusOneStepSize * 2 + (OrthographicSize + 1f) * 2f;
        }

        private void OnDisable()
        {
            if (_camera is not null)
            {
                DestroyImmediate(_camera.gameObject);
            }
        }

        public override void OnInspectorGUI()
        {
            SlicableObjectSpawnerMarker slicableObjectSpawnerMarker = (SlicableObjectSpawnerMarker)target;
            
            DrawGUI(slicableObjectSpawnerMarker);

            EditorUtility.SetDirty(target);
        }

        [DrawGizmo(GizmoType.Active | GizmoType.Pickable | GizmoType.NonSelected)]
        public static void RenderCustomGizmo(SlicableObjectSpawnerMarker slicableObjectSpawnerMarker, GizmoType gizmo)
        {
            Gizmos.color = SlicableEditorHelper.GetColor(slicableObjectSpawnerMarker);
            SlicableEditorHelper.SetColor(slicableObjectSpawnerMarker, Gizmos.color);

            SlicableObjectSpawnerData spawnerData = slicableObjectSpawnerMarker.SpawnerData;
            
            Vector2 positionInWorldFirstPoint  = GetPositionInWorld(new Vector2(spawnerData.FirstSpawnPoint.Min, spawnerData.SecondSpawnPoint.Min));
            Vector2 positionInWorldSecondPoint = GetPositionInWorld(new Vector2(spawnerData.FirstSpawnPoint.Max, spawnerData.SecondSpawnPoint.Max));
            
            Vector2 middlePoint    = (positionInWorldFirstPoint + positionInWorldSecondPoint) / 2f;
            Vector2 mainEndPoint   = GetRotatableVector(spawnerData.MainDirectionOffset);
            Vector2 firstEndPoint  = GetRotatableVector(spawnerData.MainDirectionOffset + spawnerData.FirstOffset);
            Vector2 secondEndPoint = GetRotatableVector(spawnerData.MainDirectionOffset + spawnerData.SecondOffset);
            
            Gizmos.DrawSphere(positionInWorldFirstPoint, 0.25f);
            Gizmos.DrawSphere(positionInWorldSecondPoint, 0.25f);
            Gizmos.DrawLine(positionInWorldFirstPoint, positionInWorldSecondPoint);

            Gizmos.DrawLine(middlePoint, middlePoint + mainEndPoint);
            Gizmos.DrawLine(middlePoint, middlePoint + firstEndPoint);
            Gizmos.DrawLine(middlePoint, middlePoint + secondEndPoint);
        }

        private static Vector2 GetPositionInWorld(Vector2 pointData)
        {
            float x = GetPositionFromPoint(pointData.x, HorizontalSize);
            float y = GetPositionFromPoint(pointData.y, OrthographicSize);

            return new Vector2(x, y);
        }

        private static float GetPositionFromPoint(float value, float sideLength)
        {
            if (value < 0f)
            {
                return -sideLength - (value.Abs() * sideLength);
            }

            return -sideLength + value * sideLength;
        }

        private void DrawGUI(SlicableObjectSpawnerMarker slicableObjectSpawnerMarker)
        {
            Color color = SlicableEditorHelper.GetColor(slicableObjectSpawnerMarker);
            color = EditorGUILayout.ColorField("View color: ", color);

            SlicableEditorHelper.SetColor(slicableObjectSpawnerMarker, color);
            
            base.OnInspectorGUI();
        }

        private static Vector2 GetRotatableVector(float angle)
        {
            return Quaternion.Euler(0f, 0f, angle) * Vector2.up;
        }
    }
}