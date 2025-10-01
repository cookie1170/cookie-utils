using CookieUtils.Base.Editor;
using UnityEditor;
using UnityEngine;

namespace CookieUtils.HealthSystem.Editor
{ 
    internal static class MaskSettingsWindow
    {
        private const string ProjectSettingsPath = "Project/Cookie Utils/Mask data";

        public static void OpenWindow()
        {
            SettingsService.OpenProjectSettings(ProjectSettingsPath);
        }

        [SettingsProvider]
        public static SettingsProvider CreateWindow()
        {
            return SettingsWindowBuilder.Create(HealthSystemData.Get())
                .WithPath(ProjectSettingsPath)
                .WithTitle("Mask data")
                .WithKeywords("Mask", "Health", "Hurt", "Hit", "Attack")
                .Build();
        }
    }
}