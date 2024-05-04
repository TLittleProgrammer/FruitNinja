using System.Collections.Generic;
using UnityEngine;

namespace Runtime.Infrastructure.Mouse
{
    public class IntermediateMousePositionsService : IIntermediateMousePositionsService
    {
        private const float _divide = 10;

        public Vector2[] GetIntermediateMousePositions(Vector2 firstPosition, Vector2 lastPosition, float time)
        {
            float deltaTime = time / _divide;
            float allTime = 0f;

            List<Vector2> mousePositions = new(22);
            
            while (allTime <= time)
            {
                float lerpValue = allTime / time;

                float x = Mathf.Lerp(firstPosition.x, lastPosition.x, lerpValue);
                float y = Mathf.Lerp(firstPosition.y, lastPosition.y, lerpValue);
            
                mousePositions.Add(new(x,y));

                allTime += deltaTime;
            }


            return mousePositions.ToArray();
        }
    }
}