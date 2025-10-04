using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;
#if DEBUG_CONSOLE
using IngameDebugConsole;
#endif

namespace CookieUtils
{
    /// <summary>
    /// A static class with methods and properties for debugging
    /// </summary>
    [PublicAPI]
    public static class CookieDebug
    {
        /// <summary>
        /// Is debug mode (toggled with F3) active
        /// </summary>
        public static bool IsDebugMode { get; private set; } = false;
        
        /// <summary>
        /// Invoked when debug mode gets toggled
        /// </summary>
        public static event Action<bool> OnDebugModeChanged;
        internal static event Action OnExitPlaymode;
        
        private static InputAction _debugAction;
        private static readonly List<IDebugDrawer> RegisteredObjects = new();

        /// <summary>
        /// Call to register an IDebugDrawer to get DrawDebugUI called every frame
        /// </summary>
        /// <param name="drawer">The IDebugDrawer to register</param>
        public static void Register(IDebugDrawer drawer)
        {
#if !DEBUG
            if (!Debug.isDebugBuild) return;
#endif
            if (RegisteredObjects.Contains(drawer)) return;
            
            var provider = new DebugUIBuilderProvider();
            drawer.DrawDebugUI(provider);
            RegisteredObjects.Add(drawer);
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
#if !DEBUG
            if (!Debug.isDebugBuild) return;
#endif

            _debugAction = new InputAction(binding: Keyboard.current.f3Key.path);
            _debugAction.Enable();
            _debugAction.performed += OnDebugToggled;

            InsertPlayerLoopSystem();
        }

        private static void OnDebugToggled(InputAction.CallbackContext _)
        {
            ToggleDebugMode();
        }

        private static void OnExitedPlaymode()
        {
            OnExitPlaymode?.Invoke();
            _debugAction.performed -= OnDebugToggled;
            _debugAction.Disable();
            _debugAction.Dispose();
            RegisteredObjects.Clear();
            OnExitPlaymode = null;
        }

        private static void InsertPlayerLoopSystem()
        {
            var loop = PlayerLoop.GetCurrentPlayerLoop();
            var system = new PlayerLoopSystem {
                type = typeof(CookieDebug),
                updateDelegate = DrawDebugUI,
                subSystemList = null
            };
            PlayerLoopUtils.InsertSystem<PreLateUpdate>(ref loop, in system, 0);
            PlayerLoop.SetPlayerLoop(loop);

#if UNITY_EDITOR
            UnityEditor.EditorApplication.playModeStateChanged -= PlayModeChanged;
            UnityEditor.EditorApplication.playModeStateChanged += PlayModeChanged;

            void PlayModeChanged(UnityEditor.PlayModeStateChange state)
            {
                if (state == UnityEditor.PlayModeStateChange.ExitingPlayMode) {
                    PlayerLoopUtils.RemoveSystem(ref loop, in system);
                    PlayerLoop.SetPlayerLoop(loop);
                    OnExitedPlaymode();
                }
            }
#endif
        }
        
        private static void DrawDebugUI()
        {
            if (!IsDebugMode) return;

#if !DEBUG
            if (!Debug.isDebugBuild) return;
#endif
            foreach (var drawer in RegisteredObjects) {
                var provider = new DebugUIBuilderProvider();
                drawer.DrawDebugUI(provider);
            }
        }

        /// <summary>
        /// Toggles debug mode
        /// </summary>
#if DEBUG_CONSOLE
        [ConsoleMethod("debug", "Toggles debug mode")]
#endif
        public static void ToggleDebugMode()
        {
            IsDebugMode = !IsDebugMode;
            Debug.Log($"[CookieUtils.Debug] Setting debug mode to {IsDebugMode}");
            OnDebugModeChanged?.Invoke(IsDebugMode);
        }
    }

    /// <summary>
    /// Implement this interface (and call CookieDebug.Register(this)) to draw debug UI
    /// </summary>
    [PublicAPI]
    public interface IDebugDrawer
    {
        /// <summary>
        /// Called automatically by CookieDebug in order to draw the debug ui <br/>
        /// Does not work if <see cref="CookieDebug.Register"/> isn't called earlier
        /// </summary>
        /// <param name="provider">The provider for an IDebugUIBuilder, call provider.Get(host)</param>
        /// <seealso cref="IDebugUIBuilder"/>
        public void DrawDebugUI(IDebugUIBuilderProvider provider);
    }
}
