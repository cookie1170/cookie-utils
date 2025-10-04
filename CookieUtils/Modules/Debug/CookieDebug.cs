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

        private static InputAction _debugAction;
        private static readonly List<IDebugDrawer> RegisteredObjects = new();
        private static readonly Dictionary<GameObject, Canvas> DebugUICanvases = new();

        public static void Register(IDebugDrawer drawer)
        {
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
            _debugAction.performed -= OnDebugToggled;
            _debugAction.Disable();
            _debugAction.Dispose();
            RegisteredObjects.Clear();
            DebugUICanvases.Clear();
        }

        private static void InsertPlayerLoopSystem()
        {
            var loop = PlayerLoop.GetCurrentPlayerLoop();
            var system = new PlayerLoopSystem {
                type = typeof(CookieDebug),
                updateDelegate = DrawDebugUI,
                subSystemList = null
            };
            PlayerLoopUtils.InsertSystem<PostLateUpdate>(ref loop, in system, 0);
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
            
        }

        private static Canvas GetDebugUICanvas(GameObject hostObject, bool createIfNone = true)
        {
            if (DebugUICanvases.TryGetValue(hostObject, out var canvas)) return canvas;

            if (!createIfNone) return null;

            var canvasObject = new GameObject("Debug UI Canvas");
            canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            
            return canvas;
        }

#if DEBUG_CONSOLE
        [ConsoleMethod("debug", "Toggles debug mode")]
#endif
        public static void ToggleDebugMode()
        {
            IsDebugMode = !IsDebugMode;
            Debug.Log($"[CookieUtils.Debug] Setting debug mode to {IsDebugMode}");
            for (int i = RegisteredObjects.Count - 1; i >= 0; i--) {
                var drawer = RegisteredObjects[i];

                if (drawer == null) {
                    RegisteredObjects.RemoveAt(i);
                    continue;
                }
                
                var provider = new DummyDebugUIBuilderProvider();
            
                var builder = drawer.DrawDebugUI(provider);
                
                var options = builder.GetOptions();
                
                if (!options.HostObject) {
                    RegisteredObjects.RemoveAt(i);
                    DebugUICanvases.Remove(options.HostObject);
                    continue;
                }

                var document = GetDebugUICanvas(options.HostObject, false);

                if (document) document.gameObject.SetActive(IsDebugMode);
            }
        }
    }

    [PublicAPI]
    public interface IDebugDrawer
    {
        public IDebugUIBuilder DrawDebugUI(IDebugUIBuilderProvider provider);
    }

    [PublicAPI]
    internal struct DebugUIOptions
    {
        public GameObject HostObject;
        public DebugUIPosition Position;

        public DebugUIOptions(GameObject hostObject, DebugUIPosition position)
        {
            HostObject = hostObject;
            Position = position;
        }
    }

    [PublicAPI]
    public enum DebugUIPosition
    {
        Top,
        Bottom,
        Left,
        Right
    }
}
