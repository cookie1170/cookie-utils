using System;
using System.Threading;
using UnityEngine;

namespace CookieUtils.Timer
{
    public class CountUpTimer : Timer
    {
        private readonly CancellationToken _token;

        public string GetDisplayTime(bool includeMilliseconds = true)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(Time);
            return timeSpan.ToString(includeMilliseconds ? @"hh\:mm\:ss\.ff" : @"hh\:mm\:ss");
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