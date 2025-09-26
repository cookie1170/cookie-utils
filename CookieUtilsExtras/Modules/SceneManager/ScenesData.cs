using System;
using System.Collections.Generic;
using System.Text;
using Eflatun.SceneReference;
using UnityEngine;

namespace CookieUtils.Extras.SceneManager
{
#if ALCHEMY
    [Alchemy.Inspector.DisableAlchemyEditor]
#endif
    public class ScenesData : ScriptableObject
    {
        public bool useSceneManager = true;
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
            #if UNITY_EDITOR
            if (!UnityEditor.AssetDatabase.IsValidFolder("Assets/Settings")) {
                UnityEditor.AssetDatabase.CreateFolder("Assets", "Settings");
            }
            
            if (!UnityEditor.AssetDatabase.IsValidFolder("Assets/Settings/CookieUtils"))
                UnityEditor.AssetDatabase.CreateFolder("Assets", "CookieUtils");
            
            if (!UnityEditor.AssetDatabase.IsValidFolder("Assets/Settings/CookieUtils/Resources"))
                UnityEditor.AssetDatabase.CreateFolder("Assets/Settings/CookieUtils", "Resources");

            if (!UnityEditor.AssetDatabase.AssetPathExists("Assets/Settings/CookieUtils/Resources/ScenesData.asset")) {
                var dataCreated = CreateInstance<ScenesData>();
                UnityEditor.AssetDatabase.CreateAsset(dataCreated, "Assets/Settings/CookieUtils/Resources/ScenesData.asset");
                UnityEditor.AssetDatabase.SaveAssets();
            }
            #endif

            var data = Resources.Load<ScenesData>("ScenesData");
            if (data) return data;

            Debug.LogError("[CookieUtils.Extras.SceneManager] Couldn't load ScenesData! Creating empty data to disable the scene manager");
            data = CreateInstance<ScenesData>();
            data.useSceneManager = false;

            return data;
        }


        public SceneGroup FindSceneGroupFromName(string groupName)
        {
            if (string.IsNullOrWhiteSpace(groupName)) {
                Debug.LogError("[CookieUtils.Extras.SceneManager] Group name is null or whitespace!");
                return null;
            }
            
            var targetGroup = groups.Find(g => g.name == groupName);

            if (targetGroup != null) return targetGroup;
            {
                targetGroup = groups.Find(g => string.Equals(g.name, groupName, StringComparison.CurrentCultureIgnoreCase));
                if (targetGroup != null) return targetGroup;
                
                Debug.LogError($"[CookieUtils.Extras.SceneManager] Group \"{groupName}\" not found!");
                return null;
            }
        }

#if DEBUG_CONSOLE
        [IngameDebugConsole.ConsoleMethod("groups", "Prints all scene groups")]
#endif
        public static void PrintAllGroups()
        {
            string groups = _instCached.GetAllGroups();
            Debug.Log(groups);
        }
        
        private string GetAllGroups()
        {
            var builder = new StringBuilder();
            builder.AppendLine("[CookieUtils.Extras.SceneManager]");
            builder.AppendLine("Scene groups:");
            foreach (var group in groups) {
                builder.AppendLine($"  {group.name}:");
                foreach (var scene in group.scenes) {
                    builder.AppendLine($"    {scene.Name}");
                }
            }

            return builder.ToString();
        }
    }
}
