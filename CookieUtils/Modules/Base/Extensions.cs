using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CookieUtils
{
    public static class Extensions
    {
        public static T PickRandom<T>(this IEnumerable<T> source)
        {
            List<T> sourceList = source.ToList();
            int index = Random.Range(0, sourceList.Count - 1);
            T value = sourceList[index];
            return value;
        }
    }
}
