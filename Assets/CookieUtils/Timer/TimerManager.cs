using System;
using UnityEngine;
using UnityEngine.Pool;

namespace CookieUtils.Timer
{
    public class TimerManager : MonoBehaviour
    {
        public static TimerManager Inst;

        [SerializeField] private Timer timerPrefab;
        [SerializeField] private GameObject timerContainer;

        private ObjectPool<Timer> _timerPool;

        private void Awake()
        {
            if (Inst != null) Destroy(Inst.gameObject);
            Inst = this;
            _timerPool = new(() => Instantiate(timerPrefab, parent: timerContainer.transform),
            timer => timer.gameObject.SetActive(true),
            timer => timer.gameObject.SetActive(false),
            timer => Destroy(timer.gameObject),
            true, 15, 50);
        }

        public Timer CreateTimer(float duration, bool repeat, bool ignoreTimeScale,
            bool destroyOnFinish, bool ignoreNullAction, Action onComplete = null)
        {
            Timer timer = _timerPool.Get();
            timer.Init(duration, ReleaseTimer, repeat, ignoreTimeScale, destroyOnFinish, ignoreNullAction, onComplete);
            return timer;
        }

        private void ReleaseTimer(Timer timer) => _timerPool.Release(timer);
    }
}