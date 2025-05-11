using System;
using UnityEngine;

namespace CookieUtils.Timer
{
    public class Timer : MonoBehaviour
    {
        public float TimeLeft { get; private set; }
        private float _duration;
        private bool _repeat;
        private bool _ignoreTimeScale;
        private Action _onComplete;
        
        public void Init(float duration, bool repeat = false, bool ignoreTimeScale = false, Action onComplete = null)
        {
            TimeLeft = duration;
            _duration = duration;
            _repeat = repeat;
            _ignoreTimeScale = ignoreTimeScale;
            _onComplete = onComplete;
        }

        private void Update()
        {
            TimeLeft -= _ignoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime;

            if (TimeLeft <= 0f)
            {
                _onComplete?.Invoke();
                if (_repeat)
                {
                    TimeLeft = _duration;
                }
                else Destroy(gameObject);
            }
        }
    }
}