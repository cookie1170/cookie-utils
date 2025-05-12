using System;
using CookieUtils.Timer;
using UnityEngine;

namespace CookieUtils
{
    public static class Extensionss
    {
        public static Timer.Timer CreateTimer(
            this MonoBehaviour _, float duration, bool repeat = false,
            bool ignoreTimeScale = false, bool destroyOnFinish = true,
            bool ignoreNullAction = false, Action onComplete = null
            )
        {
            return TimerManager.Inst.CreateTimer(duration, repeat, ignoreTimeScale, destroyOnFinish, ignoreNullAction,
                onComplete);
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
    }
}
