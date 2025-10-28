using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;

namespace CookieUtils.StateMachines
{
    internal static class StateMachineBootstrapper
    {
        private static PlayerLoopSystem _system;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void Initialize() {
            PlayerLoopSystem currentLoop = PlayerLoop.GetCurrentPlayerLoop();

            if (!InsertMethod<Update>(ref currentLoop, 0, StateMachineUpdater.Update)) {
                Debug.LogError("Failed to insert state machine updater into update loop!");

                return;
            }

            if (!InsertMethod<FixedUpdate>(ref currentLoop, 0, StateMachineUpdater.FixedUpdate)) {
                Debug.LogError("Failed to insert state machine updater into fixed update loop!");

                return;
            }

            PlayerLoop.SetPlayerLoop(currentLoop);

            #if UNITY_EDITOR
            EditorApplication.playModeStateChanged -= PlayModeChanged;
            EditorApplication.playModeStateChanged += PlayModeChanged;

            void PlayModeChanged(PlayModeStateChange state) {
                if (state == PlayModeStateChange.ExitingPlayMode) {
                    PlayerLoopUtils.RemoveSystem(ref currentLoop, in _system);
                    PlayerLoop.SetPlayerLoop(currentLoop);
                    StateMachineUpdater.Clear();
                }
            }
            #endif
        }

        private static bool InsertMethod<T>(
            ref PlayerLoopSystem loop,
            int index,
            PlayerLoopSystem.UpdateFunction updateAction
        ) {
            _system = new PlayerLoopSystem {
                type = typeof(StateMachineUpdater),
                subSystemList = null,
                updateDelegate = updateAction,
            };

            return PlayerLoopUtils.InsertSystem<T>(ref loop, in _system, index);
        }
    }

    internal static class StateMachineUpdater
    {
        private static readonly List<IStateMachine> StateMachines = new();

        internal static void Update() {
            foreach (IStateMachine stateMachine in StateMachines) stateMachine.Update();
        }

        internal static void FixedUpdate() {
            foreach (IStateMachine stateMachine in StateMachines) stateMachine.FixedUpdate();
        }

        internal static void Clear() {
            StateMachines.Clear();
        }

        internal static void Register(IStateMachine stateMachine) {
            StateMachines.Add(stateMachine);
        }

        internal static void Deregister(IStateMachine stateMachine) {
            StateMachines.Remove(stateMachine);
        }
    }
}