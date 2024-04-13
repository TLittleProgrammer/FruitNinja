using UnityEngine;

namespace Runtime.Constants
{
    public static class Vector2Extensions
    {
        public static float GetAngleBetweenVectorAndHorizontalAxis(this Vector2 directon)
        {
            return directon.x > 0f
                ? Vector2.SignedAngle(Vector2.right, directon)
                : Vector2.SignedAngle(Vector2.left, directon);

        } 
    }
}