
using System;
using System.Threading;
using UnityEngine;

namespace CookieUtils.Timer
{
    public class Timer
    {
        public bool IsRunning;
        
        public float TimeLeft { get; private set; }
        
        public float Duration;
        public bool Repeat;
        public bool DestroyOnComplete;
        public bool IgnoreTimeScale;
        public bool IgnoreNullAction;
        public Action OnComplete;

        private CancellationToken _token;
        private Action<Timer> _destroyAction;

        public Timer(TimerInfo info, Action<Timer> destroyAction)
        {
            _destroyAction = destroyAction;
            Duration = info.Duration;
            Repeat = info.Repeat;
            IgnoreTimeScale = info.IgnoreTimeScale;
            IgnoreNullAction = info.IgnoreNullAction;
            DestroyOnComplete = info.DestroyOnComplete;
            _token = info.CancellationToken;
            OnComplete = info.OnComplete;
            if (info.AutoStart)
                IsRunning = true;
            
            TimeLeft = Duration;
        }

        public void Restart(float newDuration)
        {
            TimeLeft = newDuration;
            IsRunning = true;
        }

        public void Restart()
        {
            TimeLeft = Duration;
            IsRunning = true;
        }

        public void Release()
        {
            _destroyAction(this);
        }

        public void Tick(float deltaTime)
        {
            if (_token.IsCancellationRequested || (OnComplete == null && !IgnoreNullAction))
            {
                _destroyAction(this);
                return;
            }

            if (!IsRunning)
                return;
            
            TimeLeft -= deltaTime;

            if (TimeLeft <= 0)
            {
                
                OnComplete?.Invoke();
                if (DestroyOnComplete)
                {
                    _destroyAction(this);
                    return;
                }

                IsRunning = false;
                
                if (Repeat)
                    Restart();
            }
        }
    }

    public struct TimerInfo
    {
        public float Duration;
        public bool Repeat;
        public bool DestroyOnComplete;
        public bool IgnoreTimeScale;
        public bool IgnoreNullAction;
        public bool AutoStart;
        
        public CancellationToken CancellationToken;
        public Action OnComplete;
    }
}