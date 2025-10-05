using CookieUtils.Base.Editor;
using UnityEditor;

namespace CookieUtils.Debugging.Editor
{
    internal static class DebuggingSettingsWindow
    {
        private const string ProjectSettingsPath = "Project/Cookie Utils/Debugging settings";
        
        [SettingsProvider]
        public static SettingsProvider CreateWindow()
        {
            return SettingsWindowBuilder.Create(DebuggingSettings.Get())
                .WithPath(ProjectSettingsPath)
                .WithTitle("Debugging settings")
                .WithKeywords("Debug", "Debugging")
                .Build();
        }
    }
}