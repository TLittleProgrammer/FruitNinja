using UnityEngine;

namespace Runtime.Infrastructure.Mouse
{
    public interface IIntermediateMousePositionsService
    {
        Vector2[] GetIntermediateMousePositions(Vector2 firstPosition, Vector2 lastPosition, float time);
    }
}