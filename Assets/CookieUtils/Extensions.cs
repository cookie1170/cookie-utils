using System;
using CookieUtils.Timer;
using UnityEngine;

namespace CookieUtils
{
    public static class Extensions
    {
        public static Timer.Timer CreateTimer(this MonoBehaviour _, float duration, bool repeat = false,
            bool ignoreTimeScale = false, bool destroyOnFinish = true, Action onComplete = null)
        {
            return TimerManager.Inst.CreateTimer(duration, repeat, ignoreTimeScale, destroyOnFinish, onComplete);
        }
    }
}
