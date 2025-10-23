using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;

namespace CookieUtils.Timers
{
    internal static class TimerInitializer
    {
        private static PlayerLoopSystem _system;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void Initialize()
        {
            var currentLoop = PlayerLoop.GetCurrentPlayerLoop();

            if (!InsertTimerManager<Update>(ref currentLoop, 0)) {
                Debug.LogError("Failed to insert timer manager into update loop!");
                return;
            }
            
            PlayerLoop.SetPlayerLoop(currentLoop);

#if UNITY_EDITOR
            EditorApplication.playModeStateChanged -= PlayModeChanged;
            EditorApplication.playModeStateChanged += PlayModeChanged;

            void PlayModeChanged(PlayModeStateChange state)
            {
                if (state == PlayModeStateChange.ExitingPlayMode) {
                    PlayerLoopUtils.RemoveSystem(ref currentLoop, in _system);
                    PlayerLoop.SetPlayerLoop(currentLoop);
                    TimerManager.Clear();
                }
            }
#endif

        }

        private static bool InsertTimerManager<T>(ref PlayerLoopSystem loop, int index)
        {
            _system = new PlayerLoopSystem {
                type = typeof(TimerManager),
                subSystemList = null,
                updateDelegate = TimerManager.UpdateTimers
            };

            return PlayerLoopUtils.InsertSystem<T>(ref loop, in _system, index);
        }
    }

    internal static class TimerManager
    {
        private static readonly List<Timer> Timers = new();

        internal static void UpdateTimers()
        {
            foreach (var timer in Timers) {
                timer.Tick();
            }
        }

        internal static void Clear()
        {
            Timers.Clear();
        }

        internal static void RegisterTimer(Timer timer)
        {
            if (Timers.Contains(timer)) return;
            Timers.Add(timer);
        }

        internal static void DeregisterTimer(Timer timer)
        {
            Timers.Remove(timer);
        }
    }
}