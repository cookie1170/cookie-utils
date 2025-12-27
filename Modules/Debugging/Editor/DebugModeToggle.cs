#if UNITY_6000_3_OR_NEWER
using UnityEditor;
using UnityEditor.Toolbars;
using UnityEngine;

namespace CookieUtils.Debugging.Editor
{
    public static class DebugModeToggle
    {
        private const string path = "Cookie Utils/Debug Mode";
        private const string iconName = "d_debug";

        [MainToolbarElement(path, defaultDockPosition = MainToolbarDockPosition.Right)]
        public static MainToolbarElement CreateToggle()
        {
            CookieDebug.DebugModeChanged -= OnDebugModeChanged;
            CookieDebug.DebugModeChanged += OnDebugModeChanged;

            bool value = CookieDebug.IsDebugMode;

            Texture2D icon = EditorGUIUtility.IconContent(iconName).image as Texture2D;
            MainToolbarContent content = new(icon, "Debug Mode");

            return new MainToolbarToggle(content, value, OnValueChanged);
        }

        private static void OnDebugModeChanged(bool value)
        {
            MainToolbar.Refresh(path);
        }

        private static void OnValueChanged(bool value)
        {
            CookieDebug.SetDebugMode(value);
            MainToolbar.Refresh(path);
        }
    }
}

#endif
