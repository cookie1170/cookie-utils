using UnityEngine;

namespace CookieUtils
{
    public static class VectorSwizzling
    {
        #region Vector2
        
        public static Vector2 xx(this Vector2 v)
        {
            return new Vector2(v.x, v.x);
        }

        public static Vector2 xy(this Vector2 v)
        {
            return new Vector2(v.x, v.y);
        }

        public static Vector2 yx(this Vector2 v)
        {
            return new Vector2(v.y, v.x);
        }

        public static Vector2 yy(this Vector2 v)
        {
            return new Vector2(v.y, v.y);
        }

        #endregion

        #region Vector3
        
        public static Vector2 xx(this Vector3 v)
        {
            return new Vector2(v.x, v.x);
        }

        public static Vector2 xy(this Vector3 v)
        {
            return new Vector2(v.x, v.y);
        }
        
        public static void xy(ref this Vector3 v, Vector2 value)
        {
            v.x = value.x;
            v.y = value.y;
        }
        
        public static void xy(ref this Vector3 v, float x, float y)
        {
            v.x = x;
            v.y = y;
        }
        
        public static Vector2 xz(this Vector3 v)
        {
            return new Vector2(v.x, v.z);
        }
        
        public static void xz(ref this Vector3 v, Vector2 value)
        {
            v.x = value.x;
            v.z = value.y;
        }
        
        public static void xz(ref this Vector3 v, float x, float y)
        {
            v.x = x;
            v.z = y;
        }

        public static Vector2 yx(this Vector3 v)
        {
            return new Vector2(v.y, v.x);
        }

        public static Vector2 yy(this Vector3 v)
        {
            return new Vector2(v.y, v.y);
        }

        public static Vector2 yz(this Vector3 v)
        {
            return new Vector2(v.y, v.z);
        }
        
        public static void yz(ref this Vector3 v, Vector2 value)
        {
            v.y = value.x;
            v.z = value.y;
        }
        
        public static void yz(ref this Vector3 v, float x, float y)
        {
            v.y = x;
            v.z = y;
        }

        public static Vector2 zx(this Vector3 v)
        {
            return new Vector2(v.z, v.x);
        }

        public static Vector2 zy(this Vector3 v)
        {
            return new Vector2(v.z, v.y);
        }

        public static Vector2 zz(this Vector3 v)
        {
            return new Vector2(v.z, v.z);
        }
        #endregion
    }
}