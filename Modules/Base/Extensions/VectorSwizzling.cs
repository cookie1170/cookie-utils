using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using UnityEngine;

// ReSharper disable InconsistentNaming

namespace CookieUtils
{
    [PublicAPI]
    public static class VectorSwizzling
    {
        #region Vector2

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 xx(this Vector2 v) => new(v.x, v.x);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 xy(this Vector2 v) => new(v.x, v.y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 yx(this Vector2 v) => new(v.y, v.x);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 yy(this Vector2 v) => new(v.y, v.y);

        #endregion

        #region Vector3

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 xx(this Vector3 v) => new(v.x, v.x);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 xy(this Vector3 v) => new(v.x, v.y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void xy(ref this Vector3 v, Vector2 value)
        {
            v.x = value.x;
            v.y = value.y;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void xy(ref this Vector3 v, float x, float y)
        {
            v.x = x;
            v.y = y;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 xz(this Vector3 v) => new(v.x, v.z);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void xz(ref this Vector3 v, Vector2 value)
        {
            v.x = value.x;
            v.z = value.y;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void xz(ref this Vector3 v, float x, float y)
        {
            v.x = x;
            v.z = y;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 yx(this Vector3 v) => new(v.y, v.x);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 yy(this Vector3 v) => new(v.y, v.y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 yz(this Vector3 v) => new(v.y, v.z);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void yz(ref this Vector3 v, Vector2 value)
        {
            v.y = value.x;
            v.z = value.y;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void yz(ref this Vector3 v, float x, float y)
        {
            v.y = x;
            v.z = y;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 zx(this Vector3 v) => new(v.z, v.x);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 zy(this Vector3 v) => new(v.z, v.y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 zz(this Vector3 v) => new(v.z, v.z);

        #endregion
    }
}
