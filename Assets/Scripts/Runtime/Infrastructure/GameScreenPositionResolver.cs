using System;
using Cysharp.Threading.Tasks;
using Runtime.SlicableObjects.Spawner;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Runtime.Infrastructure
{
    public class GameScreenPositionResolver : IAsyncInitializable
    {
        private float _resolution;
        private float _orthographicSize;
        private float _horizontalSize;
        private float _horizontalPlusOneStepSize;
        
        public async UniTask AsyncInitialize()
        {
            Camera camera = Camera.main;

            _orthographicSize            = camera.orthographicSize;
            _resolution                  = (float)Screen.width / Screen.height;
            _horizontalSize              = _resolution * _orthographicSize;
            _horizontalPlusOneStepSize   = _resolution * (_orthographicSize + 1);
            
            await UniTask.CompletedTask;
        }

        public Vector2 GetRandomPositionBetweenTwoPercents(float firstPercent, float secondPercent, SideType sideType)
        {
            float value = sideType switch
            {
                SideType.Left or SideType.Right => GetLerpBetweenToValues(-_orthographicSize, _orthographicSize, Random.Range(firstPercent, secondPercent)),
                SideType.Bottom => GetLerpBetweenToValues(-_horizontalSize, _horizontalSize, Random.Range(firstPercent, secondPercent)),
                
                _ => 0f
            };

            if (sideType is SideType.Bottom)
            {
                return new Vector2(value, -(_orthographicSize - 1));
            }

            Vector2 result = new Vector2(_horizontalPlusOneStepSize, value);

            if (sideType is SideType.Left)
            {
                result.x *= -1;
            }
            
            return result;
        }

        private float GetLerpBetweenToValues(float a, float b, float t)
        {
            return Mathf.Lerp(a, b, t);
        }
    }
}