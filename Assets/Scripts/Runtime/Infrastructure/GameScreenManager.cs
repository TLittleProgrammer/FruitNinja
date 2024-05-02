using Cysharp.Threading.Tasks;
using Runtime.Extensions;
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

        public async UniTask AsyncInitialize(Camera camera)
        {
            _orthographicSize           = camera.orthographicSize;
            _resolution                 = (float)Screen.width / Screen.height;
            _horizontalSize             = _resolution * _orthographicSize;
            _horizontalPlusOneStepSize  = _resolution * (_orthographicSize + 1);

            await UniTask.CompletedTask;
        }

        public float GetHorizontalSizeWithStep()
        {
            return _horizontalPlusOneStepSize;
        }

        public float GetOrthographicSize()
        {
            return _orthographicSize;
        }

        public Vector2 GetRandomPositionBetweenTwoPercents(SlicableObjectSpawnerData spawnerData)
        {
            Vector2 positionInWorldFirstPoint  = GetPositionInWorld(new Vector2(spawnerData.FirstSpawnPoint.Min, spawnerData.SecondSpawnPoint.Min));
            Vector2 positionInWorldSecondPoint = GetPositionInWorld(new Vector2(spawnerData.FirstSpawnPoint.Max, spawnerData.SecondSpawnPoint.Max));

            return new Vector2(
                Random.Range(positionInWorldFirstPoint.x, positionInWorldSecondPoint.x),
                Random.Range(positionInWorldFirstPoint.y, positionInWorldSecondPoint.y)
                );
        }
        
        private Vector2 GetPositionInWorld(Vector2 pointData)
        {
            float x = GetPositionFromPoint(pointData.x, _horizontalSize);
            float y = GetPositionFromPoint(pointData.y, _orthographicSize);

            return new Vector2(x, y);
        }

        private float GetPositionFromPoint(float value, float sideLength)
        {
            if (value < 0f)
            {
                return -sideLength - (value.Abs() * sideLength);
            }

            return -sideLength + value * sideLength;
        }
    }
}