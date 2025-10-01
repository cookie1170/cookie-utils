using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CookieUtils.HealthSystem
{
    public class HealthSystemData : ScriptableObject
    {
        private const string Path = "Assets/Settings/CookieUtils/Resources/HealthSystemData.asset";
        
        public List<string> masks = new();
        public LayerMask wallMasks;
        
        public static HealthSystemData Get()
        {
            var data = Resources.Load<HealthSystemData>("HealthSystemData");
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
            
            data = CreateInstance<HealthSystemData>();
            #if UNITY_EDITOR
            AssetDatabase.CreateAsset(data, Path);
            AssetDatabase.SaveAssets();
            #endif

            return data;
        }
    }
}
