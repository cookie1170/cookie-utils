using CookieUtils.Base.Editor;
using UnityEditor;
using UnityEngine;

namespace CookieUtils.HealthSystem.Editor
{ 
    internal static class MaskSettingsWindow
    {
        private const string Path = "Assets/Settings/CookieUtils/MaskData.asset";
        private const string ProjectSettingsPath = "Project/Cookie Utils/Mask data";

        public static void OpenWindow()
        {
            SettingsService.OpenProjectSettings(ProjectSettingsPath);
        }
        
        public static MaskData GetMask()
        {
            var data = AssetDatabase.LoadAssetAtPath<MaskData>(Path);
            if (data) return data;

            if (!AssetDatabase.IsValidFolder("Assets/Settings")) {
                AssetDatabase.CreateFolder("Assets", "Settings");
            }
            
            if (!AssetDatabase.IsValidFolder("Assets/Settings/CookieUtils")) {
                AssetDatabase.CreateFolder("Assets/Settings", "CookieUtils");
            }
            
            data = ScriptableObject.CreateInstance<MaskData>();
            AssetDatabase.CreateAsset(data, Path);
            AssetDatabase.SaveAssets();

            return data;
        }

        [SettingsProvider]
        public static SettingsProvider CreateWindow()
        {
            return SettingsWindowBuilder.Create(GetMask())
                .WithPath(ProjectSettingsPath)
                .WithTitle("Mask data")
                .WithKeywords("Mask", "Health", "Hurt", "Hit", "Attack")
                .Build();
        }
    }
}