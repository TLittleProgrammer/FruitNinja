using UnityEngine;

namespace Runtime.Extensions
{
    public static class DataExtensions
    {
        public static string ToJson(this object obj) => 
            JsonUtility.ToJson(obj);

        public static TResult ToDeserialized<TResult>(this string json) =>
            JsonUtility.FromJson<TResult>(json);
    }
}