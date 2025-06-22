
using System;
using System.Threading;
using UnityEngine;

namespace CookieUtils.Timer
{
    public class CountdownTimer : Timer
    {
        public float Duration;
        public bool Repeat;
        public bool DestroyOnComplete;
        public bool IgnoreNullAction;
        public Action OnComplete;

        private readonly CancellationToken _token;

        public CountdownTimer(MonoBehaviour host, float duration, Action onComplete = null, bool repeat = false,
            bool destroyOnComplete = true, bool ignoreTimeScale = false, bool ignoreNullAction = false,
            bool autoStart = true)
        {
            _token = host.destroyCancellationToken;
            Duration = duration;
            OnComplete = onComplete;
            Repeat = repeat;
            DestroyOnComplete = destroyOnComplete;
            IgnoreTimeScale = ignoreTimeScale;
            IgnoreNullAction = ignoreNullAction;
            if (autoStart)
                Start();

            TimerManager.RegisterTimer(this);
        }

        public override void Start()
        {
            if (IsRunning) return;
            base.Start();
            Restart();
        }

        public void Restart(float newDuration)
        {
            Time = newDuration;
            IsRunning = true;
        }

        public void Restart()
        {
            Restart(Duration);
        }

        public override void Tick(float deltaTime)
        {
            if (_token.IsCancellationRequested || (OnComplete == null && !IgnoreNullAction))
            {
                Release();
                return;
            }

            if (!IsRunning)
                return;
            
            Time -= deltaTime;

            if (Time <= 0)
            {
                
                OnComplete?.Invoke();
                if (DestroyOnComplete)
                {
                    Release();
                    return;
                }

                IsRunning = false;
                
                if (Repeat)
                    Restart();
            }
        }
    }
}