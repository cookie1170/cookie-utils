using System.Collections.Generic;
using Eflatun.SceneReference;
using UnityEditor;
using UnityEngine;

namespace CookieUtils.Extras.Scenes
{
    public class ScenesData : ScriptableObject
    {
        public SceneReference bootstrapScene;
        public List<SceneGroup> groups;
        public string startingGroup;

        public static ScenesData GetScenesData()
        {
            if (!AssetDatabase.AssetPathExists($"Assets/{DataFolder}/{DataName}")) {
                if (!AssetDatabase.IsValidFolder($"Assets/{DataFolder}")) {
                    AssetDatabase.CreateFolder("Assets", DataFolder);
                }
                
                AssetDatabase.CreateAsset(CreateInstance<ScenesData>(), $"Assets/{DataFolder}/{DataName}");
            }

            return AssetDatabase.LoadAssetAtPath<ScenesData>($"Assets/{DataFolder}/{DataName}");
        }

        private const string DataFolder = "CookieUtils";
        private const string DataName = "ScenesData.asset";
    }
}
