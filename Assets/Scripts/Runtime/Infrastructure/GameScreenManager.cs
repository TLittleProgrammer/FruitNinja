using Cysharp.Threading.Tasks;
using Runtime.Infrastructure.SlicableObjects.Spawner;
using UnityEngine;

namespace Runtime.Infrastructure
{
    public class GameScreenManager : IAsyncInitializable<Camera>
    {
        private float _resolution;
        private float _orthographicSize;
        private float _horizontalSize;
        private float _horizontalPlusOneStepSize;
        private Rect _cameraRect;
        private Camera _camera;

        public async UniTask AsyncInitialize(Camera camera)
        {
            _camera                     = camera;
            _orthographicSize           = camera.orthographicSize;
            _resolution                 = (float)Screen.width / Screen.height;
            _horizontalSize             = _resolution * _orthographicSize;
            _horizontalPlusOneStepSize  = _resolution * (_orthographicSize + 1);
            _cameraRect                 = camera.rect;

            _cameraRect.width += 400f;
            _cameraRect.height += 400f;
            
            await UniTask.CompletedTask;
        }

        public Vector2 GetRandomPositionBetweenTwoPercents(SlicableObjectSpawnerData spawnerData)
        {
            float constantPositionValue = GetPositionBySide(spawnerData.SideType);
            
            return GetPositionInWorld(constantPositionValue, spawnerData.SideType, Random.Range(spawnerData.FirstSpawnPoint, spawnerData.SecondSpawnPoint));
        }

        private float GetPositionBySide(SideType sideType)
        {
            return sideType switch
            {
                SideType.Bottom => -_orthographicSize - 1,
                SideType.Left   => -_horizontalPlusOneStepSize,
                SideType.Right  => _horizontalPlusOneStepSize,
                _               => 0f
            };
        }

        private Vector2 GetPositionInWorld(float constantPositionValue, SideType sideType, float lerpValue)
        {
            return sideType switch
            {
                SideType.Bottom => new Vector2(GetLerp(sideType, lerpValue), constantPositionValue),
                SideType.Left   => new Vector2(constantPositionValue, GetLerp(sideType, lerpValue)),
                SideType.Right  => new Vector2(constantPositionValue, GetLerp(sideType, lerpValue)),
                _               => Vector2.zero
            };
        }

        public Vector2 GetRotatableVectorPoint(float angle)
        {
            return Quaternion.Euler(0f, 0f, angle) * Vector2.up;
        }

        public float GetOrthographicSize()
        {
            return _orthographicSize;
        }

        public bool WorldPositionAtScreenRect(Vector2 point)
        {
            Vector2 screenPoint = _camera.WorldToViewportPoint(point);
            return _cameraRect.Contains(screenPoint);
        }

        private float GetLerp(SideType sideType, float lerpValue)
        {
            return sideType switch
            {
                SideType.Bottom => Mathf.Lerp(-_horizontalSize, _horizontalSize, lerpValue),
                
                _ => Mathf.Lerp(-_orthographicSize, _orthographicSize, lerpValue) 
            };
        }
    }
}