using System;
using CookieUtils.Timer;
using UnityEngine;

namespace CookieUtils
{
    public static class Extensions
    {
        private static Timer.Timer CreateTimer(this MonoBehaviour _, float duration, bool repeat = false, bool ignoreTimeScale = false,
            Action onComplete = null)
        {
            return TimerManager.Inst.CreateTimer(duration, repeat, ignoreTimeScale, onComplete);
        }
    }
}
