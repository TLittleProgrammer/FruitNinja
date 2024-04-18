using System.Collections.Generic;
using UnityEngine;

namespace Editor.SlicableObjectSpawner
{
    public static class SlicableEditorHelper
    {
        private static Dictionary<int, Color> Colors = new();

        public static void SetColor(object obj, Color color)
        {
            int hashCode = obj.GetHashCode();

            if (Colors.ContainsKey(hashCode))
            {
                Colors[hashCode] = color;
                
                return;
            }
            
            Colors.Add(hashCode, color);
        }
        
        public static Color GetColor(object obj)
        {
            int hashCode = obj.GetHashCode();

            if (Colors.TryGetValue(hashCode, out Color color))
            {
                return color;
            }
            
            return new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);
        }
    }
}