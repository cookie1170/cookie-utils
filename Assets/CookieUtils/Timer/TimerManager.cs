using System;
using UnityEngine;

namespace CookieUtils.Timer
{
    public class TimerManager : MonoBehaviour
    {
        public static TimerManager Inst;

        private void Awake()
        {
            if (Inst != null) Destroy(Inst.gameObject);
            Inst = this;
        }

        public Timer CreateTimer(float duration, bool repeat = false, bool ignoreTimeScale = false, Action onComplete = null)
        {
            Timer timer = Instantiate(new GameObject().AddComponent<Timer>());
            timer.Init(duration, repeat, ignoreTimeScale, onComplete);
            return timer;
        }
    }
}