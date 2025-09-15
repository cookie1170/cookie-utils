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

        public static Vector2 With(this Vector2 vec, float? x = null, float? y = null)
        {
            vec.x = x ?? vec.x;
            vec.y = y ?? vec.y;
            return vec;
        }
        
        public static Vector3 With(this Vector3 vec, float? x = null, float? y = null, float? z = null)
        {
            vec.x = x ?? vec.x;
            vec.y = y ?? vec.y;
            vec.z = z ?? vec.z;
            return vec;
        }
        
        public static Vector2 WithAdd(this Vector2 vec, float x = 0, float y = 0)
        {
            vec.x += x;
            vec.y += y;
            return vec;
        }
        
        public static Vector3 WithAdd(this Vector3 vec, float x = 0, float y = 0, float z = 0)
        {
            vec.x += x;
            vec.y += y;
            vec.z += z;
            return vec;
        }

        public static float Normalized(this float f, float min, float max) => (f - min) / (max - min);
        public static float Normalized(this float f, float max) => f / max;
        
        public static float ClampAngle(this float angle, float min, float max) =>
            Mathf.Clamp((angle <= 180) ? angle : -(360 - angle), min, max);
    }
}
