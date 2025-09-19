using System;
using System.Collections.Generic;
using Eflatun.SceneReference;
using UnityEditor;
using UnityEngine;

namespace CookieUtils.Extras.Scenes
{
#if ALCHEMY
    [Alchemy.Inspector.DisableAlchemyEditor]
#endif
    public class ScenesData : ScriptableObject
    {
        public SceneGroupReference startingGroup;
        public SceneReference bootstrapScene;
        public List<SceneGroup> groups;

        private static ScenesData _instCached;

        public static ScenesData GetScenesData()
        {
            if (_instCached) return _instCached;

            _instCached = LoadSceneData();
            return _instCached;
        }

        private static ScenesData LoadSceneData()
        {
            if (!AssetDatabase.AssetPathExists($"Assets/{DataFolder}/{DataName}")) {
                if (!AssetDatabase.IsValidFolder($"Assets/{DataFolder}")) {
                    AssetDatabase.CreateFolder("Assets", DataFolder);
                }
                
                AssetDatabase.CreateAsset(CreateInstance<ScenesData>(), $"Assets/{DataFolder}/{DataName}");
            }

            return AssetDatabase.LoadAssetAtPath<ScenesData>($"Assets/{DataFolder}/{DataName}");
        }


        public SceneGroup FindSceneGroupFromName(string groupName)
        {
            if (string.IsNullOrWhiteSpace(groupName)) {
                Debug.LogError("[CookieUtils.Extras.Scenes] Group name is null or whitespace!");
                return null;
            }
            
            var targetGroup = groups.Find(g => g.name == groupName);

            if (targetGroup != null) return targetGroup;
            {
                targetGroup = groups.Find(g => string.Equals(g.name, groupName, StringComparison.CurrentCultureIgnoreCase));
                if (targetGroup != null) return targetGroup;
                
                Debug.LogError($"[CookieUtils.Extras.Scenes] Group \"{groupName}\" not found!");
                return null;
            }
        }

        private const string DataFolder = "CookieUtils";
        private const string DataName = "ScenesData.asset";
    }
}
