using System.Collections.Generic;
using UnityEngine;

namespace Runtime.Extensions
{
    public static class ListExtensions
    {
        public static TResult GetRandomValue<TResult>(this List<TResult> list) where TResult : class
        {
            if (list.Count == 0)
            {
                return null;
            }

            return list[Random.Range(0, list.Count)];
        }
    }
}