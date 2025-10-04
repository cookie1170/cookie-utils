using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CookieUtils.Debugging
{
    public class DebuggingSettings : ScriptableObject
    {
        private const string Path = "Assets/Settings/CookieUtils/Resources/DebuggingSettings.asset";

        public float refreshTime = 0.1f;
        
        public static DebuggingSettings Get()
        {
            var data = Resources.Load<DebuggingSettings>("HealthSystemData");
            if (data) return data;
            
#if UNITY_EDITOR
            if (!AssetDatabase.IsValidFolder("Assets/Settings")) {
                AssetDatabase.CreateFolder("Assets", "Settings");
            }
            
            if (!AssetDatabase.IsValidFolder("Assets/Settings/CookieUtils")) {
                AssetDatabase.CreateFolder("Assets/Settings", "CookieUtils");
            }

            if (!AssetDatabase.IsValidFolder("Assets/Settings/CookieUtils/Resources")) {
                AssetDatabase.CreateFolder("Assets/Settings/CookieUtils", "Resources");
            }
#endif
            
            data = CreateInstance<DebuggingSettings>();
            
#if UNITY_EDITOR
            AssetDatabase.CreateAsset(data, Path);
            AssetDatabase.SaveAssets();
#endif

            return data;
        }
    }
}
