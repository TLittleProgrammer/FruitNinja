﻿using UnityEngine;

namespace Runtime.Extensions
{
    public static class FloatExtensions
    {
        public static float ConvertToRadians(this float angleValue)
        {
            return angleValue * Mathf.Deg2Rad;
        }

        public static float Abs(this float value)
        {
            return value > 0f ? value : -value;
        }
    }
}