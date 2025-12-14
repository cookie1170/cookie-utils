using UnityEngine;
using UnityEngine.InputSystem;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CookieUtils.Debugging
{
    [SettingsObject(
        "DebuggingSettings",
        "Debug settings",
        "Cookie Utils/Debug settings",
        "Debug", "Debugging", "UI"
    )]
    public class DebuggingSettings : SettingsObject<DebuggingSettings>
    {
        [Tooltip("The time between each check for the mouse intersection")]
        public float mouseCheckTime = 0.2f;

        [Tooltip("The time until the ui gets hidden after you stop hovering")]
        public float hideTime = 0.5f;

        [Tooltip("An action to toggle debug mode")]
        public InputAction toggleDebugMode = new("Toggle Debug Mode", InputActionType.Button, "<keyboard>/backquote");

        #if UNITY_EDITOR
        [SettingsProvider]
        private static SettingsProvider ProvideSettings() => GetSettings();
        #endif
    }
}