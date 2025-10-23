using System;
using System.Collections.Generic;
using System.Text;
using Eflatun.SceneReference;
using JetBrains.Annotations;
using UnityEngine;

namespace CookieUtils.Extras.SceneManager
{
#if ALCHEMY
    [Alchemy.Inspector.DisableAlchemyEditor]
#endif
    [PublicAPI]
    [SettingsObject(
            "ScenesSettings",
            "Scenes settings",
            "Cookie Utils/Scenes",
            "Scenes", "Cookie Utils", "Scene", "Groups", "Group"
        )]
    public class ScenesSettings : SettingsObject<ScenesSettings>
    {
        public bool useSceneManager = true;
        public SceneGroupReference startingGroup;
        public SceneReference bootstrapScene;
        public List<SceneGroup> groups;

        public SceneGroup FindSceneGroupFromName(string groupName)
        {
            if (string.IsNullOrWhiteSpace(groupName)) {
                Debug.LogError("[CookieUtils.Extras.SceneManager] Group name is null or whitespace!");
                return null;
            }

            var targetGroup = groups.Find(g => g.name == groupName);

            if (targetGroup != null) return targetGroup;
            {
                targetGroup = groups.Find(g =>
                    string.Equals(g.name, groupName, StringComparison.CurrentCultureIgnoreCase));
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
            string groups = Get().GetAllGroups();
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

#if UNITY_EDITOR
        [UnityEditor.SettingsProvider]
        private static UnityEditor.SettingsProvider ProvideSettings() => GetSettings();
#endif
    }
}
