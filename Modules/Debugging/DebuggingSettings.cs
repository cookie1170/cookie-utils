using UnityEngine;
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
        [Tooltip("The time between ui getting refreshed")]
        public float refreshTime = 0.1f;

        [Tooltip("The time between each check for the mouse intersection")]
        public float mouseCheckTime = 0.2f;

        [Tooltip("The time until the ui gets hidden after you stop hovering")]
        public float hideTime = 0.5f;

        #if UNITY_EDITOR
        [SettingsProvider]
        private static SettingsProvider ProvideSettings() => GetSettings();
        #endif
    }
}