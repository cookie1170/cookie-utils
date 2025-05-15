using System;
using System.Diagnostics;
using System.Threading;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace CookieUtils.Timer
{
    public class Timer : MonoBehaviour
    {
        [NonSerialized] public float Duration;
        [NonSerialized] public Action OnComplete;

        public string DisplayTime => TimeLeft.ToString("#.##");
        [NonSerialized] public bool IsRunning;
        public float TimeLeft { get; private set; }

        private Action<Timer> _releaseAction;
        private CancellationToken _cancelToken;
        private bool _repeat;
        private bool _ignoreTimeScale;
        private bool _destroyOnFinish;
        private bool _ignoreNullAction;
        
        public void Init(float duration, Action<Timer> releaseAction, CancellationToken cancelToken, bool repeat, bool ignoreTimeScale,
            bool destroyOnFinish, bool ignoreNullAction, Action onComplete)
        {
            Duration = duration;
            TimeLeft = duration;
            OnComplete = onComplete;
            IsRunning = true;
            _releaseAction = releaseAction;
            _repeat = repeat;
            _ignoreTimeScale = ignoreTimeScale;
            _destroyOnFinish = destroyOnFinish;
            _ignoreNullAction = ignoreNullAction;
            _cancelToken = cancelToken;
        }

        public void Restart(float time = -1)
        {
            TimeLeft = Mathf.Approximately(time, -1) ? Duration : time;
            IsRunning = true;
        }

        private void Update()
        {
            if (!IsRunning) return;
            
            TimeLeft -= _ignoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime;
            
            if (TimeLeft <= 0f)
            {
                if (_cancelToken.IsCancellationRequested)
                {
                    Release();
                    return;
                }
                TimeLeft = 0f;
                IsRunning = false;
                if (!_ignoreNullAction && OnComplete == null)
                {
                    Release();
                    return;
                }

                OnComplete?.Invoke();
                if (_repeat) Restart();
                if (_destroyOnFinish && !_repeat) Release();
            }
        }

        public void Release()
        {
            OnComplete = null;
            _releaseAction?.Invoke(this);
        }
    }
}