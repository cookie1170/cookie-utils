using System.Reflection;
using JetBrains.Annotations;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CookieUtils
{
    [PublicAPI]
    public abstract class SettingsObject<T> : ScriptableObject
        where T : ScriptableObject
    {
        private static T _instCached;

        public static T Get()
        {
            if (_instCached)
                return _instCached;

            var attribute = (SettingsObjectAttribute)
                typeof(T).GetCustomAttribute(typeof(SettingsObjectAttribute));

            _instCached = Resources.Load<T>($"CookieUtils/Settings/{attribute.PathName}");

            if (_instCached)
                return _instCached;

            _instCached = CreateInstance<T>();

#if UNITY_EDITOR
            CreatePath();
            AssetDatabase.CreateAsset(
                _instCached,
                $"Assets/CookieUtils/Resources/CookieUtils/Settings/{attribute.PathName}.asset"
            );
            AssetDatabase.SaveAssets();
#endif

            return _instCached;
        }

#if UNITY_EDITOR
        private static void CreatePath()
        {
            string[] pathElements = { "CookieUtils", "Resources", "CookieUtils", "Settings" };
            string constructedPath = "Assets";

            foreach (string element in pathElements)
            {
                string newPath = $"{constructedPath}/{element}";

                if (!AssetDatabase.AssetPathExists(newPath))
                    AssetDatabase.CreateFolder(constructedPath, element);

                constructedPath = $"{constructedPath}/{element}";
            }
        }

        protected static SettingsProvider GetSettings()
        {
            var attribute = (SettingsObjectAttribute)
                typeof(T).GetCustomAttribute(typeof(SettingsObjectAttribute));

            T instance = Get();

            return GetProvider(
                instance,
                attribute.SettingsPath,
                attribute.DisplayName,
                attribute.Keywords
            );
        }

        public static void OpenWindow()
        {
            var attribute = (SettingsObjectAttribute)
                typeof(T).GetCustomAttribute(typeof(SettingsObjectAttribute));

            SettingsService.OpenProjectSettings($"Project/{attribute.SettingsPath}");
        }

        private static SettingsProvider GetProvider(
            ScriptableObject target,
            string path,
            string name,
            string[] keywords = null
        )
        {
            SettingsProvider provider = new($"Project/{path}", SettingsScope.Project, keywords)
            {
                label = name,
                activateHandler = (_, rootElement) =>
                {
                    Label titleLabel = new(name)
                    {
                        style =
                        {
                            fontSize = 18,
                            paddingLeft = 16,
                            paddingTop = 2,
                            unityFontStyleAndWeight = FontStyle.Bold,
                        },
                    };
                    rootElement.Add(titleLabel);

                    InspectorElement inspector = new(target);

                    PropertyField scriptField = inspector
                        .Query<PropertyField>()
                        .Where(f => f.bindingPath == "m_Script")
                        .First();
                    scriptField?.parent?.Remove(scriptField);

                    rootElement.Add(inspector);
                },
            };

            return provider;
        }
#endif
    }
}
