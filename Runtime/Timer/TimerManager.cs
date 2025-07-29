using System.Collections.Generic;
using UnityEngine;

namespace CookieUtils.Runtime.Timer
{
    public class TimerManager : MonoBehaviour
    {
        private static TimerManager _inst;
        
        private readonly List<Timer> _timers = new();

        private void Awake()
        {
            if (_inst) Destroy(_inst.gameObject);
            _inst = this;
        }

        private void Update()
        {
            float deltaTime = Time.deltaTime;
            float unscaledDeltaTime = Time.unscaledDeltaTime;

            for (int i = _timers.Count - 1; i >= 0; i--)
            {
                Timer timer = _timers[i];
                timer.Tick(timer.IgnoreTimeScale ? unscaledDeltaTime : deltaTime);
            }
        }

        public static void RegisterTimer(Timer timer)
        {
            if (!_inst)
            {
                Debug.LogError("Timer Manager's instance is null, returning");
                return;
            }

            _inst._timers.Add(timer);
        }

        public static void ReleaseTimer(Timer timer)
        {
            _inst._timers.Remove(timer);
        }
    }
}