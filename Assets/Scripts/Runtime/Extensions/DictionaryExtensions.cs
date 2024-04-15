using System.Collections.Generic;

namespace Runtime.Extensions
{
    public static class DictionaryExtensions
    {
        public static void AddItemWhereValueIsList<TKey, TValue>(this Dictionary<TKey, List<TValue>> dictionary, TKey key, TValue value)
        {
            InitializeMemory(dictionary, key);

            dictionary[key].Add(value);
        }
        
        public static void AddItemWhereValueIsList<TKey, TValue>(this Dictionary<TKey, List<TValue>> dictionary, TKey key, List<TValue> value)
        {
            InitializeMemory(dictionary, key);

            dictionary[key].AddRange(value);
        }

        private static void InitializeMemory<TKey, TValue>(Dictionary<TKey, List<TValue>> dictionary, TKey key)
        {
            if (!dictionary.ContainsKey(key))
            {
                dictionary.Add(key, new());
            }
        }
    }
}