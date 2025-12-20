using JetBrains.Annotations;
using UnityEngine;

namespace CookieUtils
{
    [PublicAPI]
    public static class NumberExtensions
    {
        /// <summary>
        ///     Alias for Mathf.Approximately
        /// </summary>
        public static bool Is(this float f1, float f2) => Mathf.Approximately(f1, f2);

        /// <summary>
        ///     Squares the value
        /// </summary>
        /// <param name="v">The value to square</param>
        /// <returns>v squared</returns>
        public static float Squared(this float v) => v * v;
    }
}
