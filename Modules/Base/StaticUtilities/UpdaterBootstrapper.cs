using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CookieUtils
{
    internal static class UpdaterBootstrapper
    {
        private static PlayerLoopSystem _system;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void Initialize()
        {
            PlayerLoopSystem currentLoop = PlayerLoop.GetCurrentPlayerLoop();

            if (!InsertMethod<Update>(ref currentLoop, 0, Updater.Update))
            {
                Debug.LogError("[CookieUtils - Error] Failed to insert updater into update loop!");

                return;
            }

            if (!InsertMethod<FixedUpdate>(ref currentLoop, 0, Updater.FixedUpdate))
            {
                Debug.LogError(
                    "[CookieUtils - Error] Failed to insert updater into fixed update loop!"
                );

                return;
            }

            PlayerLoop.SetPlayerLoop(currentLoop);

#if UNITY_EDITOR
            EditorApplication.playModeStateChanged -= PlayModeChanged;
            EditorApplication.playModeStateChanged += PlayModeChanged;

            void PlayModeChanged(PlayModeStateChange state)
            {
                if (state == PlayModeStateChange.ExitingPlayMode)
                {
                    PlayerLoopUtils.RemoveSystem(ref currentLoop, in _system);
                    PlayerLoop.SetPlayerLoop(currentLoop);
                    Updater.Clear();
                }
            }
#endif
        }

        private static bool InsertMethod<T>(
            ref PlayerLoopSystem loop,
            int index,
            PlayerLoopSystem.UpdateFunction updateAction
        )
        {
            _system = new PlayerLoopSystem
            {
                type = typeof(Updater),
                subSystemList = null,
                updateDelegate = updateAction,
            };

            return PlayerLoopUtils.InsertSystem<T>(ref loop, in _system, index);
        }
    }
}
