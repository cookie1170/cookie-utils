using System;
using System.Linq;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Object = UnityEngine.Object;
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
        internal static DebugSettings DebuggingSettings;
        private static InputAction _lockOnAction;
        private static InputAction _debugAction;

        /// <summary>
        ///     Is debug mode active <br />
        ///     Makes <see cref="IDebugDrawer" />s draw debug ui
        /// </summary>
        public static bool IsDebugMode { get; private set; } = false;

        /// <summary>
        ///     Invoked when debug mode gets toggled
        /// </summary>
        /// <seealso cref="IsDebugMode" />
        public static event Action<bool> DebugModeChanged;

        internal static event Action ExitedPlaymode;
        internal static event Action LockOnAttempted;
        internal static event Action DebugUICleared;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
#if !DEBUG
            if (!Debug.isDebugBuild)
                return;
#endif

            DebuggingSettings = DebugSettings.Get();
            _debugAction = DebuggingSettings.toggleDebugMode;
            _debugAction.Enable();
            _debugAction.performed += OnDebugToggled;
            _lockOnAction = new InputAction(binding: Mouse.current.leftButton.path);
            _lockOnAction.Enable();
            _lockOnAction.performed += OnLockOn;

#if UNITY_EDITOR
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
#endif
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange change)
        {
            if (change == PlayModeStateChange.ExitingPlayMode)
                OnExitedPlaymode();
        }

        private static void OnDebugToggled(InputAction.CallbackContext _)
        {
            ToggleDebugMode();
        }

        private static void OnLockOn(InputAction.CallbackContext _)
        {
            if (Keyboard.current.ctrlKey.isPressed)
                LockOnAttempted?.Invoke();
        }

        private static void OnExitedPlaymode()
        {
            ExitedPlaymode?.Invoke();
            _debugAction.performed -= OnDebugToggled;
            _lockOnAction.performed -= OnLockOn;
            _debugAction.Dispose();
            _lockOnAction.Dispose();
            DebuggingSettings = null;
            ExitedPlaymode = null;
            LockOnAttempted = null;
            DebugUICleared = null;
        }

        /// <summary>
        ///     Toggles debug mode
        /// </summary>
        /// <seealso cref="IsDebugMode" />
#if DEBUG_CONSOLE
        [ConsoleMethod("debug", "Toggles debug mode")]
#endif
        public static void ToggleDebugMode()
        {
            IsDebugMode = !IsDebugMode;
            Debug.Log($"[CookieUtils.Debug] Setting debug mode to {IsDebugMode}");
            DebugModeChanged?.Invoke(IsDebugMode);
            RefreshDebugUI();
        }

        /// <summary>
        ///     Clears and (if <see cref="IsDebugMode">Debug mode</see> is enabled) recreates the debug UI
        /// </summary>
#if DEBUG_CONSOLE
        [ConsoleMethod("refresh-debug-ui", "Refreshes the debug UI")]
#endif
        public static void RefreshDebugUI()
        {
            DebugUICleared?.Invoke();

            if (!IsDebugMode)
                return;

            var debugDrawers = Object
                .FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
                .OfType<IDebugDrawer>();

            foreach (IDebugDrawer obj in debugDrawers)
                obj.SetUpDebugUI(new UGUIDebugUI_BuilderProvider());

            EventSystem eventSystem = Object.FindAnyObjectByType<EventSystem>(
                FindObjectsInactive.Exclude
            );
            if (!eventSystem || !eventSystem.isActiveAndEnabled)
            {
                EventSystemHandler handler = new GameObject(
                    "Debug UI Event system"
                ).AddComponent<EventSystemHandler>();
                handler.OnDebugModeChanged(true);
            }
        }
    }

    /// <summary>
    ///     Implement this interface to draw debug UI
    /// </summary>
    [PublicAPI]
    public interface IDebugDrawer
    {
        /// <summary>
        ///     Called automatically by CookieDebug in order to set up the debug ui
        /// </summary>
        /// <param name="provider">The provider for an <see cref="IDebugUI_Builder" /></param>
        /// <seealso cref="IDebugUI_Builder" />
        /// <seealso cref="IDebugUI_BuilderProvider.GetFor(GameObject)" />
        public void SetUpDebugUI(IDebugUI_BuilderProvider provider);
    }
}
