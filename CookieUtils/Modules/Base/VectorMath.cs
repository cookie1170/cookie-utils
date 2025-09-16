using UnityEngine;

namespace CookieUtils
{
    public static class VectorMath
    {
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

        public static void Set(ref this Vector3 vec, float? x = null, float? y = null, float? z = null)
        {
            vec = vec.With(x, y, z);
        }
        
        public static void Add(ref this Vector3 vec, float x = 0, float y = 0, float z = 0)
        {
            vec.x += x;
            vec.y += y;
            vec.z += z;
        }
    }
}