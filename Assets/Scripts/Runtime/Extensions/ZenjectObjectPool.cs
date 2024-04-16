using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Runtime.Extensions
{
    public static class ZenjectObjectPool
    {
        public static TContract GetInactiveObject<TContract>(this IEnumerable<TContract> objects) where TContract : MonoBehaviour
        {
            return objects.First(x => x.gameObject.activeInHierarchy is false);
        }
    }
}