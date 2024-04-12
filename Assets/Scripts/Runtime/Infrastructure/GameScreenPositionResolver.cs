using Cysharp.Threading.Tasks;
using Runtime.SlicableObjects.Spawner;
using UnityEngine;

namespace Runtime.Infrastructure
{
    public class GameScreenPositionResolver : IAsyncInitializable<Camera>
    {
        private float _resolution;
        private float _orthographicSize;
        private float _horizontalSize;
        private float _horizontalPlusOneStepSize;

        public async UniTask AsyncInitialize(Camera camera)
        {
            _orthographicSize            = camera.orthographicSize;
            _resolution                  = (float)Screen.width / Screen.height;
            _horizontalSize              = _resolution * _orthographicSize;
            _horizontalPlusOneStepSize   = _resolution * (_orthographicSize + 1);
            
            await UniTask.CompletedTask;
        }

        public Vector2 GetRandomPositionBetweenTwoPercents(SlicableObjectSpawnerData spawnerData)
        {
            float constantPositionValue = GetPositionBySide(spawnerData.SideType);
            
            return GetPositionInWorld(constantPositionValue, spawnerData.SideType, Random.Range(spawnerData.FirstSpawnPoint, spawnerData.SecondSpawnPoint));
        }

        public Vector2 GetMiddlePoint(SlicableObjectSpawnerData spawnerData)
        {
            float positionBySide = GetPositionBySide(spawnerData.SideType);
            
            Vector2 positionInWorldFirstPoint  = GetPositionInWorld(positionBySide, spawnerData.SideType, spawnerData.FirstSpawnPoint);
            Vector2 positionInWorldSecondPoint = GetPositionInWorld(positionBySide, spawnerData.SideType, spawnerData.SecondSpawnPoint);

            return (positionInWorldFirstPoint + positionInWorldSecondPoint) / 2f;
        }

        public Vector2 GetRotatableVectorPoint(Vector2 firstPoint, Vector2 secondPoint, float angle)
        {
            float distance = Vector2.Distance(firstPoint, secondPoint);
            
            float deltaY = Mathf.Sin(angle) * distance;
            float deltaX = Mathf.Cos(angle) * distance;
            
            return new Vector2(secondPoint.x - deltaX, secondPoint.y - deltaY);
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