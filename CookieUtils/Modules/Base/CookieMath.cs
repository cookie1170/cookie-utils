using UnityEngine;

namespace CookieUtils
{
    public static class CookieMath
    {
        public static float Remap(float v, float sourceMin, float sourceMax, float targetMin, float targetMax)
        {
            float t = Mathf.InverseLerp(sourceMin, sourceMax, v);
            return Mathf.Lerp(targetMin, targetMax, t);
        }
        
        public static float Remap(float v, float sourceMax, float targetMin, float targetMax)
        {
            float t = Mathf.InverseLerp(0, sourceMax, v);
            return Mathf.Lerp(targetMin, targetMax, t);
        }
        
        public static float Remap(float v, float sourceMax, float targetMax)
        {
            float t = Mathf.InverseLerp(0, sourceMax, v);
            return Mathf.Lerp(0, targetMax, t);
        }
        
        public static float Normalize(float v, float min, float max) => (v - min) / (max - min);
        
		public static float Normalize(float v, float max) => v / max;

        public static float ClampAngle(float v, float min, float max) =>
            Mathf.Clamp(v <= 180 ? v : -(360 - v), min, max);
    }
}