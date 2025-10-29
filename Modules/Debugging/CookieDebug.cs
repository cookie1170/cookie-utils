using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;
#if DEBUG_CONSOLE
using IngameDebugConsole;
#endif

namespace CookieUtils.Debugging
{
    /// <summary>
    ///     A static class with methods and properties for debugging
    /// </summary>
    [PublicAPI]
    public static class CookieDebug
    {
        internal static DebuggingSettings DebuggingSettings;
        internal static readonly List<IDebugDrawer> RegisteredObjects = new();
        private static InputAction _lockOnAction;
        private static InputAction _debugAction;

        /// <summary>
        ///     Is debug mode (toggled with backquote) active <br />
        ///     Makes <see cref="IDebugDrawer" />s draw debug ui
        /// </summary>
        public static bool IsDebugMode { get; private set; } = false;

        /// <summary>
        ///     Invoked when debug mode gets toggled
        /// </summary>
        /// <seealso cref="IsDebugMode" />
        public static event Action<bool> OnDebugModeChanged;

        internal static event Action OnExitPlaymode;
        internal static event Action OnLockedOn;
        internal static event Action OnDebugUICleared;

        /// <summary>
        ///     Call to register an <see cref="IDebugDrawer" /> to draw debug ui
        /// </summary>
        /// <param name="drawer">The <see cref="IDebugDrawer" /> to register</param>
        public static void Register(IDebugDrawer drawer) {
            #if !DEBUG
            if (!Debug.isDebugBuild) return;
            #endif
            if (RegisteredObjects.Contains(drawer)) return;

            RegisteredObjects.Add(drawer);
            RefreshDebugUI();
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init() {
            #if !DEBUG
            if (!Debug.isDebugBuild) return;
            #endif

            DebuggingSettings = DebuggingSettings.Get();
            _debugAction = new InputAction(binding: Keyboard.current.backquoteKey.path);
            _debugAction.Enable();
            _debugAction.performed += OnDebugToggled;
            _lockOnAction = new InputAction(binding: Mouse.current.leftButton.path);
            _lockOnAction.Enable();
            _lockOnAction.performed += OnLockOn;
        }

        private static void OnDebugToggled(InputAction.CallbackContext _) {
            ToggleDebugMode();
        }

        private static void OnLockOn(InputAction.CallbackContext _) {
            if (Keyboard.current.ctrlKey.isPressed)
                OnLockedOn?.Invoke();
        }

        private static void OnExitedPlaymode() {
            OnExitPlaymode?.Invoke();
            _debugAction.performed -= OnDebugToggled;
            _lockOnAction.performed -= OnLockOn;
            _debugAction.Dispose();
            _lockOnAction.Dispose();
            RegisteredObjects.Clear();
            DebuggingSettings = null;
            OnExitPlaymode = null;
            OnLockedOn = null;
            OnDebugUICleared = null;
        }

        /// <summary>
        ///     Toggles debug mode
        /// </summary>
        /// <seealso cref="IsDebugMode" />
        #if DEBUG_CONSOLE
        [ConsoleMethod("debug", "Toggles debug mode")]
        #endif
        public static void ToggleDebugMode() {
            IsDebugMode = !IsDebugMode;
            Debug.Log($"[CookieUtils.Debug] Setting debug mode to {IsDebugMode}");
            OnDebugModeChanged?.Invoke(IsDebugMode);
            RefreshDebugUI();
        }

        /// <summary>
        ///     Clears and (if <see cref="IsDebugMode">Debug mode</see> is enabled) recreates the debug UI
        /// </summary>
        #if DEBUG_CONSOLE
        [ConsoleMethod("refresh-debug-ui", "Refreshes the debug UI")]
        #endif
        public static void RefreshDebugUI() {
            OnDebugUICleared?.Invoke();

            if (!IsDebugMode) return;

            foreach (IDebugDrawer obj in RegisteredObjects) obj?.SetUpDebugUI(new UGUIDebugUIBuilderProvider());
        }
    }

    /// <summary>
    ///     Implement this interface (and call <see cref="CookieDebug.Register" />) to draw debug UI
    /// </summary>
    [PublicAPI]
    public interface IDebugDrawer
    {
        /// <summary>
        ///     Called automatically by CookieDebug in order to set up the debug ui <br />
        ///     Does not work if <see cref="CookieDebug.Register" /> isn't called earlier
        /// </summary>
        /// <param name="provider">The provider for an IDebugUIBuilder</param>
        /// <seealso cref="IDebugUIBuilder" />
        /// <seealso cref="IDebugUIBuilderProvider.GetFor(GameObject)" />
        public void SetUpDebugUI(IDebugUIBuilderProvider provider);
    }
}