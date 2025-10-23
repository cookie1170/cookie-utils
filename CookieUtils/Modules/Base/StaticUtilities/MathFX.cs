// ReSharper disable file IdentifierTypo because weird math words

using JetBrains.Annotations;
using UnityEngine;

namespace CookieUtils
{
    [PublicAPI]
    public static class Mathfx
    {
        public static float CustomCurve(AnimationCurve anim, float value) => anim.Evaluate(value);

        public static float Hermite(float start, float end, float value) => Mathf.Lerp(
            start, end, value * value * (3.0f - 2.0f * value)
        );

        public static Vector2 Hermite(Vector2 start, Vector2 end, float value) => new(
            Hermite(start.x, end.x, value), Hermite(start.y, end.y, value)
        );

        public static Vector3 Hermite(Vector3 start, Vector3 end, float value) =>
            new(
                Hermite(start.x, end.x, value), Hermite(start.y, end.y, value),
                Hermite(start.z, end.z, value)
            );

        public static float Sinerp(float start, float end, float value) =>
            Mathf.Lerp(start, end, Mathf.Sin(value * Mathf.PI * 0.5f));

        public static Vector2 Sinerp(Vector2 start, Vector2 end, float value) =>
            new(
                Mathf.Lerp(start.x, end.x, Mathf.Sin(value * Mathf.PI * 0.5f)),
                Mathf.Lerp(start.y, end.y, Mathf.Sin(value * Mathf.PI * 0.5f))
            );

        public static Vector3 Sinerp(Vector3 start, Vector3 end, float value) =>
            new(
                Mathf.Lerp(start.x, end.x, Mathf.Sin(value * Mathf.PI * 0.5f)),
                Mathf.Lerp(start.y, end.y, Mathf.Sin(value * Mathf.PI * 0.5f)),
                Mathf.Lerp(start.z, end.z, Mathf.Sin(value * Mathf.PI * 0.5f))
            );

        public static float Coserp(float start, float end, float value) => Mathf.Lerp(
            start, end, 1.0f - Mathf.Cos(value * Mathf.PI * 0.5f)
        );

        public static Vector2 Coserp(Vector2 start, Vector2 end, float value) => new(
            Coserp(start.x, end.x, value), Coserp(start.y, end.y, value)
        );

        public static Vector3 Coserp(Vector3 start, Vector3 end, float value) =>
            new(
                Coserp(start.x, end.x, value), Coserp(start.y, end.y, value),
                Coserp(start.z, end.z, value)
            );

        public static float Berp(float start, float end, float value) {
            value = Mathf.Clamp01(value);
            value = (Mathf.Sin(value * Mathf.PI * (0.2f + 2.5f * value * value * value)) * Mathf.Pow(1f - value, 2.2f) +
                     value) * (1f + 1.2f * (1f - value));

            return start + (end - start) * value;
        }

        public static Vector2 Berp(Vector2 start, Vector2 end, float value) => new(
            Berp(start.x, end.x, value), Berp(start.y, end.y, value)
        );

        public static Vector3 Berp(Vector3 start, Vector3 end, float value) => new(
            Berp(start.x, end.x, value), Berp(start.y, end.y, value), Berp(start.z, end.z, value)
        );

        public static float SmoothStep(float x, float min, float max) {
            x = Mathf.Clamp(x, min, max);
            float v1 = (x - min) / (max - min);
            float v2 = (x - min) / (max - min);

            return -2 * v1 * v1 * v1 + 3 * v2 * v2;
        }

        public static Vector2 SmoothStep(Vector2 vec, float min, float max) => new(
            SmoothStep(vec.x, min, max), SmoothStep(vec.y, min, max)
        );

        public static Vector3 SmoothStep(Vector3 vec, float min, float max) => new(
            SmoothStep(vec.x, min, max), SmoothStep(vec.y, min, max), SmoothStep(vec.z, min, max)
        );

        public static float Lerp(float start, float end, float value) => (1.0f - value) * start + value * end;

        public static Vector3 NearestPoint(Vector3 lineStart, Vector3 lineEnd, Vector3 point) {
            Vector3 lineDirection = Vector3.Normalize(lineEnd - lineStart);
            float closestPoint = Vector3.Dot(point - lineStart, lineDirection);

            return lineStart + closestPoint * lineDirection;
        }

        public static Vector3 NearestPointStrict(Vector3 lineStart, Vector3 lineEnd, Vector3 point) {
            Vector3 fullDirection = lineEnd - lineStart;
            Vector3 lineDirection = Vector3.Normalize(fullDirection);
            float closestPoint = Vector3.Dot(point - lineStart, lineDirection);

            return lineStart + Mathf.Clamp(closestPoint, 0.0f, Vector3.Magnitude(fullDirection)) * lineDirection;
        }

        public static float Bounce(float x) => Mathf.Abs(Mathf.Sin(6.28f * (x + 1f) * (x + 1f)) * (1f - x));

        public static Vector2 Bounce(Vector2 vec) => new(Bounce(vec.x), Bounce(vec.y));

        public static Vector3 Bounce(Vector3 vec) => new(Bounce(vec.x), Bounce(vec.y), Bounce(vec.z));

        public static bool Approx(float val, float about, float range) => Mathf.Abs(val - about) < range;

        public static bool Approx(Vector3 val, Vector3 about, float range) =>
            (val - about).sqrMagnitude < range * range;

        public static float Clerp(float start, float end, float value) {
            float min = 0.0f;
            float max = 360.0f;
            float half = Mathf.Abs((max - min) / 2.0f);
            float retval;
            float diff;

            if (end - start < -half) {
                diff = (max - start + end) * value;
                retval = start + diff;
            } else if (end - start > half) {
                diff = -(max - end + start) * value;
                retval = start + diff;
            } else {
                retval = start + (end - start) * value;
            }

            return retval;
        }
    }
}