using System;
using Runtime.SlicableObjects.Spawner;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Editor.SlicableObjectSpawner
{
    [CustomEditor(typeof(SlicableObjectSpawnerMarker))]
    public class SlicableObjectSpawnerMarkerEditor : UnityEditor.Editor
    {
        private static float OrthographicSize;
        private static float Resolution;
        private static float HorizontalSize;
        private static float HorizontalPlusOneStepSize;
        private static float CameraHeight;
        private static float CameraWidth;
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

            CameraHeight = OrthographicSize * 2;
            CameraWidth  = HorizontalSize * 2;
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

            float positionBySide = GetPositionBySide(slicableObjectSpawnerMarker.SideType);
            Vector2 positionInWorldFirstPoint  = GetPositionInWorld(positionBySide, slicableObjectSpawnerMarker.SideType, slicableObjectSpawnerMarker.FirstSpawnPointPercent);
            Vector2 positionInWorldSecondPoint = GetPositionInWorld(positionBySide, slicableObjectSpawnerMarker.SideType, slicableObjectSpawnerMarker.SecondSpawnPointPercent);
            
            Gizmos.DrawSphere(positionInWorldFirstPoint, 0.25f);
            Gizmos.DrawSphere(positionInWorldSecondPoint, 0.25f);
            Gizmos.DrawLine(positionInWorldFirstPoint, positionInWorldSecondPoint);

            Vector2 middlePoint = (positionInWorldFirstPoint + positionInWorldSecondPoint) / 2f;

            Vector2 directionToMiddleCameraPoint = (Vector2.zero - middlePoint).normalized;

            Vector2 mainEndPoint   = GetRotatableVector(middlePoint, directionToMiddleCameraPoint, slicableObjectSpawnerMarker.MainDirectionOffset).normalized;
            Vector2 firstEndPoint  = GetRotatableVector(middlePoint, directionToMiddleCameraPoint, slicableObjectSpawnerMarker.MainDirectionOffset + slicableObjectSpawnerMarker.FirstOffset).normalized;
            Vector2 secondEndPoint = GetRotatableVector(middlePoint, directionToMiddleCameraPoint, slicableObjectSpawnerMarker.MainDirectionOffset + slicableObjectSpawnerMarker.SecondOffset).normalized;
            
            Gizmos.DrawLine(middlePoint, middlePoint + mainEndPoint);
            Gizmos.DrawLine(middlePoint, middlePoint + firstEndPoint);
            Gizmos.DrawLine(middlePoint, middlePoint + secondEndPoint);
        }

        private void DrawGUI(SlicableObjectSpawnerMarker slicableObjectSpawnerMarker)
        {
            Color color = SlicableEditorHelper.GetColor(slicableObjectSpawnerMarker);
            color = EditorGUILayout.ColorField("View color: ", color);

            SlicableEditorHelper.SetColor(slicableObjectSpawnerMarker, color);
            
            base.OnInspectorGUI();
        }

        private static Vector2 GetPositionInWorld(float constantPositionValue, SideType sideType, float lerpValue)
        {
            return sideType switch
            {
                SideType.Bottom => new Vector2(GetLerp(sideType, lerpValue), constantPositionValue),
                SideType.Left   => new Vector2(constantPositionValue, GetLerp(sideType, lerpValue)),
                SideType.Right  => new Vector2(constantPositionValue, GetLerp(sideType, lerpValue)),
                _               => Vector2.zero
            };
        }

        private static float GetPositionBySide(SideType sideType)
        {
            return sideType switch
            {
                SideType.Bottom => -OrthographicSize - 1,
                SideType.Left   => -HorizontalPlusOneStepSize,
                SideType.Right  => HorizontalPlusOneStepSize,
                _               => 0f
            };
        }

        private static float GetLerp(SideType sideType, float lerpValue)
        {
            return sideType switch
            {
                SideType.Bottom => Mathf.Lerp(-HorizontalSize, HorizontalSize, lerpValue),
                
                _ => Mathf.Lerp(-OrthographicSize, OrthographicSize, lerpValue) 
            };
        }

        private static Vector2 GetRotatableVector(Vector2 firstPoint, Vector2 secondPoint, float angle)
        {
            float distance = Vector2.Distance(firstPoint, secondPoint);
            
            float deltaY = Mathf.Sin(angle) * distance;
            float deltaX = Mathf.Cos(angle) * distance;
            
            return new Vector2(secondPoint.x - deltaX, secondPoint.y - deltaY);
        }
    }
}