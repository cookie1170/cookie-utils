using System.Collections.Generic;
using UnityEngine;

namespace CookieUtils.Timer
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

        public static Timer CreateTimer(TimerInfo info)
        {
            if (!_inst)
            {
                Debug.LogError("Timer manager's instance is null, aborting timer creation");
                return null;
            }
            
            Timer timer = new Timer(info, ReleaseTimer);
            _inst._timers.Add(timer);
            
            return timer;
        }

        private static void ReleaseTimer(Timer timer)
        {
            _inst._timers.Remove(timer);
        }
    }
}