using System;
using UnityEngine;
using UnityEngine.Pool;

namespace CookieUtils.Timer
{
    public class TimerManager : MonoBehaviour
    {
        public static TimerManager Inst;

        [SerializeField] private Timer timerPrefab;
        [SerializeField] private Transform timerContainer;

        private ObjectPool<Timer> _timerPool;

        private void Awake()
        {
            if (Inst != null) Destroy(Inst.gameObject);
            Inst = this;
            _timerPool = new(() => Instantiate(timerPrefab, parent: timerContainer),
            timer => timer.gameObject.SetActive(true),
            timer => timer.gameObject.SetActive(false),
            timer => Destroy(timer.gameObject),
            true, 15, 50);
        }

        public static Timer CreateTimer(float duration, bool repeat, bool ignoreTimeScale,
            bool destroyOnFinish, bool ignoreNullAction, Action onComplete = null)
        {
            if (!Inst)
            {
                Debug.LogError($"Timer manager's instance is null, aborting timer creation");
                return null;
            }
            Timer timer = Inst._timerPool.Get();
            timer.Init(duration, ReleaseTimer, repeat, ignoreTimeScale, destroyOnFinish, ignoreNullAction, onComplete);
            return timer;
        }

        private static void ReleaseTimer(Timer timer) => Inst._timerPool.Release(timer);
    }
}