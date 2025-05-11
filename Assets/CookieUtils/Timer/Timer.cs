using System;
using UnityEngine;

namespace CookieUtils.Timer
{
    public class Timer : MonoBehaviour
    {
        public float TimeLeft { get; private set; }
        public Action OnComplete;

        private Action<Timer> _releaseAction;
        private float _duration;
        private bool _repeat;
        private bool _ignoreTimeScale;
        private bool _destroyOnFinish;
        
        public void Init(float duration, Action<Timer> releaseAction, bool repeat = false, bool ignoreTimeScale = false, bool destroyOnFinish = true, Action onComplete = null)
        {
            TimeLeft = duration;
            _releaseAction = releaseAction;
            _duration = duration;
            _repeat = repeat;
            _ignoreTimeScale = ignoreTimeScale;
            _destroyOnFinish = destroyOnFinish;
            OnComplete = onComplete;
        }

        public void Restart() =>
            TimeLeft = _duration;

        private void Update()
        {
            TimeLeft -= _ignoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime;

            if (TimeLeft <= 0f)
            {
                OnComplete?.Invoke();
                if (_repeat)
                {
                    Restart();
                }

                if (_destroyOnFinish && !_repeat) Release();
            }
        }

        public void Release()
        {
            OnComplete = null;
            _releaseAction.Invoke(this);
        }
    }
}