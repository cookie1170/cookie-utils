using System;
using UnityEngine;

namespace CookieUtils.Timer
{
    public static class TimerExtensions
    {
        public static Timer CreateTimer(
            this MonoBehaviour _, float duration, bool repeat = false,
            bool ignoreTimeScale = false, bool destroyOnFinish = true,
            bool ignoreNullAction = false, Action onComplete = null
        )
        {
            return TimerManager.Inst.CreateTimer(duration, repeat, ignoreTimeScale, destroyOnFinish, ignoreNullAction,
                onComplete);
        }
    }
}