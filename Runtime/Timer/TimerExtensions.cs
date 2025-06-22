using System;
using UnityEngine;

namespace CookieUtils.Timer
{
    public static class TimerExtensions
    {
        public static Timer CreateTimer(
            this MonoBehaviour behaviour, float duration, bool repeat = false,
            bool ignoreTimeScale = false, bool destroyOnComplete = true,
            bool ignoreNullAction = false, Action onComplete = null, bool autoStart = true
        )
        {
            TimerInfo info = new TimerInfo();
            info.Duration = duration;
            info.Repeat = repeat;
            info.IgnoreTimeScale = ignoreTimeScale;
            info.IgnoreNullAction = ignoreNullAction;
            info.DestroyOnComplete = destroyOnComplete;
            info.CancellationToken = behaviour.destroyCancellationToken;
            info.OnComplete = onComplete;
            info.AutoStart = autoStart;

            return TimerManager.CreateTimer(info);
        }
    }
}