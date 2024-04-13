using UnityEngine;

namespace Runtime.Constants
{
    public static class FloatExtensions
    {
        public static float ConvertToRadians(this float angleValue)
        {
            return Mathf.PI / 180 * angleValue;
        }
    }
}