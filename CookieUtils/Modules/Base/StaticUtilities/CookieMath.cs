using JetBrains.Annotations;
using UnityEngine;

namespace CookieUtils
{
    [PublicAPI]
    public static class CookieMath
    {
        /// <summary>
        ///     Remaps the value <c>v</c> from source range to target range
        /// </summary>
        /// <example>
        ///     <code>
        /// float sourceMin = 10;
        /// float sourceMax = 20;
        /// float targetMin = 30;
        /// float targetMax = 50;
        /// float v = 15; // Halfway between sourceMin (10) and sourceMax (20)
        /// float result = Remap(v, sourceMin, sourceMax, targetMin, targetMax);
        /// Debug.Log(result); // 40, which is halfway between targetMin (30) and targetMax (50)
        /// </code>
        /// </example>
        /// <param name="v">The value to remap</param>
        /// <param name="sourceMin">The low end of the source range</param>
        /// <param name="sourceMax">The high end of the source range</param>
        /// <param name="targetMin">The low end of the target range</param>
        /// <param name="targetMax">The high end of the target range</param>
        /// <returns>The remapped value</returns>
        [Pure]
        public static float Remap(float v, float sourceMin, float sourceMax, float targetMin, float targetMax)
        {
            float t = Mathf.InverseLerp(sourceMin, sourceMax, v);
            return Mathf.Lerp(targetMin, targetMax, t);
        }

        /// <summary>
        ///     Remaps the value <c>v</c> from source range (starting at 0) to target range
        /// </summary>
        /// <example>
        ///     <code>
        /// float sourceMax = 20;
        /// float targetMin = 30;
        /// float targetMax = 50;
        /// float v = 10; // Halfway between 0 and sourceMax (20)
        /// float result = Remap(v, sourceMax, targetMin, targetMax);
        /// Debug.Log(result); // 40, which is halfway between targetMin (30) and targetMax (50)
        /// </code>
        /// </example>
        /// <param name="v">The value to remap</param>
        /// <param name="sourceMax">The high end of the source range</param>
        /// <param name="targetMin">The low end of the target range</param>
        /// <param name="targetMax">The high end of the target range</param>
        /// <returns>The remapped value</returns>
        [Pure]
        public static float Remap(float v, float sourceMax, float targetMin, float targetMax)
        {
            return Remap(v, 0, sourceMax, targetMin, targetMax);
        }


        /// <summary>
        ///     Remaps the value <c>v</c> from source range (starting at 0) to target range
        /// </summary>
        /// <example>
        ///     <code>
        /// float sourceMax = 20;
        /// float targetMax = 50;
        /// float v = 10; // Halfway between 0 and sourceMax (20)
        /// float result = Remap(v, sourceMax, targetMax);
        /// Debug.Log(result); // 25, which is halfway between 0 and targetMax (50)
        /// </code>
        /// </example>
        /// <param name="v">The value to remap</param>
        /// <param name="sourceMax">The high end of the source range</param>
        /// <param name="targetMax">The high end of the target range</param>
        /// <returns>The remapped value</returns>
        [Pure]
        public static float Remap(float v, float sourceMax, float targetMax)
        {
            return Remap(v, 0, sourceMax, 0, targetMax);
        }

        /// <summary>
        ///     Is the number <c>i</c> odd
        /// </summary>
        /// <param name="i">the number to check</param>
        /// <returns>True if the number is odd, else false</returns>
        [Pure]
        public static bool IsOdd(int i)
        {
            return i % 2 == 1;
        }

        /// <summary>
        ///     Is the number <c>i</c> even
        /// </summary>
        /// <param name="i">the number to check</param>
        /// <returns>True if the number is even, else false</returns>
        [Pure]
        public static bool IsEven(int i)
        {
            return i % 2 == 0;
        }

        /// <summary>
        ///     Clamps the value to be above min
        /// </summary>
        /// <param name="value">The value to clamp</param>
        /// <param name="min">The minimum value</param>
        /// <returns>min if value is less than min, else value</returns>
        [Pure]
        public static float AtLeast(float value, float min)
        {
            return Mathf.Max(value, min);
        }

        /// <summary>
        ///     Clamps the value to be below max
        /// </summary>
        /// <param name="value">The value to clamp</param>
        /// <param name="max">The maximum value</param>
        /// <returns>max if value is greater than max, else value</returns>
        [Pure]
        public static float AtMost(float value, float max)
        {
            return Mathf.Min(value, max);
        }

        /// <inheritdoc cref="AtLeast(float, float)" />
        [Pure]
        public static int AtLeast(int value, int min)
        {
            return Mathf.Max(value, min);
        }

        /// <inheritdoc cref="AtMost(float, float)" />
        [Pure]
        public static int AtMost(int value, int max)
        {
            return Mathf.Min(value, max);
        }

        /// <summary>
        ///     Clamps the Euler angle <c>v</c> (in degrees)
        /// </summary>
        /// <param name="v">The Euler angle to clamp</param>
        /// <param name="min">The min value</param>
        /// <param name="max">The max value</param>
        /// <returns>The Euler angle clamped between min and max</returns>
        [Pure]
        public static float ClampAngle(float v, float min, float max)
        {
            return Mathf.Clamp(v <= 180 ? v : -(360 - v), min, max);
        }
    }
}