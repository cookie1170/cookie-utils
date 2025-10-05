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

namespace CookieUtils.Debugging
{
    /// <summary>
    /// A static class with methods and properties for debugging
    /// </summary>
    [PublicAPI]
    public static class CookieDebug
    {
        /// <summary>
        /// Is debug mode (toggled with backquote) active <br/>
        /// Makes <see cref="IDebugDrawer"/>s draw debug ui
        /// </summary>
        public static bool IsDebugMode { get; private set; } = false;

        /// <summary>
        /// Invoked when debug mode gets toggled
        /// </summary>
        /// <seealso cref="IsDebugMode"/>
        public static event Action<bool> OnDebugModeChanged;
        
        internal static DebuggingSettings DebuggingSettings;
        internal static event Action OnExitPlaymode;
        internal static event Action OnLockedOn;
        internal static readonly List<IDebugDrawer> RegisteredObjects = new();
        private static InputAction _lockOnAction;
        private static InputAction _debugAction;
        private static float _timeSinceLastRender = 0f;
        private static float _refreshTime = float.PositiveInfinity;

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

            RegisteredObjects.Add(drawer);
            var provider = new DebugUIBuilderProvider(RegisteredObjects.Count - 1);
            drawer.DrawDebugUI(provider);
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
#if !DEBUG
            if (!Debug.isDebugBuild) return;
#endif

            DebuggingSettings = DebuggingSettings.Get();
            _refreshTime = DebuggingSettings.refreshTime;
            _debugAction = new InputAction(binding: Keyboard.current.backquoteKey.path);
            _debugAction.Enable();
            _debugAction.performed += OnDebugToggled;
            _lockOnAction = new InputAction(binding: Mouse.current.leftButton.path);
            _lockOnAction.Enable();
            _lockOnAction.performed += OnLockOn;
            
            InsertPlayerLoopSystem();
        }

        private static void OnDebugToggled(InputAction.CallbackContext _)
        {
            ToggleDebugMode();
        }

        private static void OnLockOn(InputAction.CallbackContext _)
        {
            if (Keyboard.current.ctrlKey.isPressed)
                OnLockedOn?.Invoke();
        }

        private static void OnExitedPlaymode()
        {
            OnExitPlaymode?.Invoke();
            _debugAction.performed -= OnDebugToggled;
            _lockOnAction.performed -= OnLockOn;
            _debugAction.Dispose();
            _lockOnAction.Dispose();
            RegisteredObjects.Clear();
            DebuggingSettings = null;
            OnExitPlaymode = null;
            OnLockedOn = null;
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
            _timeSinceLastRender += Time.unscaledDeltaTime;
            if (_timeSinceLastRender < _refreshTime) return;

            _timeSinceLastRender = 0f;

            for (int i = RegisteredObjects.Count - 1; i >= 0; i--) {
                var drawer = RegisteredObjects[i];
                if (drawer == null) {
                    RegisteredObjects.RemoveAt(i);
                    continue;
                }

                var provider = new DebugUIBuilderProvider(i);
                drawer.DrawDebugUI(provider);
            }
        }

        /// <summary>
        /// Toggles debug mode/>
        /// </summary>
        /// <seealso cref="IsDebugMode"/>"/>
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
