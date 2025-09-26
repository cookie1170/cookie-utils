using CookieUtils.Base.Editor;
using UnityEditor;

namespace CookieUtils.Extras.SceneManager.Editor
{
    internal static class ScenesSettingsWindow
    {
        private const string ProjectSettingsPath = "Project/Cookie Utils/Scenes";
        
        [SettingsProvider]
        public static SettingsProvider CreateWindow()
        {
            return SettingsWindowBuilder.Create(ScenesData.GetScenesData())
                .WithPath(ProjectSettingsPath)
                .WithTitle("Scenes")
                .WithKeywords("Scenes", "Scene", "Groups", "Group")
                .Build();
        }

        public static void OpenWindow()
        {
            SettingsService.OpenProjectSettings(ProjectSettingsPath);    
        }
    }
}