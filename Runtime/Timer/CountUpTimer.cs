using System;
using System.Threading;
using UnityEngine;

namespace CookieUtils.Runtime.Timer
{
    public class CountUpTimer : Timer
    {
        private readonly CancellationToken _token;

        public static string FormatTime(float time, bool includeMilliseconds = true)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(time);
            string formatString = $"{(time > 3600 ? @"hh\:" : "")}{(time > 60 ? @"mm\:" : "")}ss{(includeMilliseconds ? @"\.ff" : "")}";
            return timeSpan.ToString(formatString);
        }

        public string GetDisplayTime(bool includeMilliseconds = true)
        {
            return FormatTime(Time, includeMilliseconds);
        }
        
        public override void Tick(float deltaTime)
        {
            if (_token.IsCancellationRequested)
            {
                Release();
            } else
            {
                if (IsRunning) Time += deltaTime;
            }
        }

        public CountUpTimer(MonoBehaviour host, bool ignoreTimeScale = false, bool autoStart = true)
        {
            _token = host.destroyCancellationToken;
            IgnoreTimeScale = ignoreTimeScale;
            if (autoStart) Start();
            TimerManager.RegisterTimer(this);
        }
    }
}