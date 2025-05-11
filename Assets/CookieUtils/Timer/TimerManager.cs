using System;
using UnityEngine;
using UnityEngine.Pool;

namespace CookieUtils.Timer
{
    public class TimerManager : MonoBehaviour
    {
        public static TimerManager Inst;

        [SerializeField] private Timer timerPrefab;

        private ObjectPool<Timer> _timerPool;

        private void Awake()
        {
            if (Inst != null) Destroy(Inst.gameObject);
            Inst = this;
            _timerPool = new(() => Instantiate(timerPrefab, parent: transform),
            timer => timer.gameObject.SetActive(true),
            timer => timer.gameObject.SetActive(false),
            timer => Destroy(timer.gameObject),
            false, 10, 20);
        }

        public Timer CreateTimer(float duration, bool repeat = false, bool ignoreTimeScale = false, bool destroyOnFinish = true, Action onComplete = null)
        {
            Timer timer = _timerPool.Get();
            timer.Init(duration, ReleaseTimer, repeat, ignoreTimeScale, destroyOnFinish, onComplete);
            return timer;
        }

        private void ReleaseTimer(Timer timer) => _timerPool.Release(timer);
    }
}