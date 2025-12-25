using System.IO;
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
        private static readonly string BasePath = Path.Join("CookieUtils", "Resources");
        private static readonly string ResourcesPath = "Settings";
        private static T _instCached;

        public static T Get()
        {
            if (_instCached)
                return _instCached;

            var attribute = typeof(T).GetCustomAttribute<SettingsObjectAttribute>();

            string pathName = typeof(T).Name;
            _instCached = Resources.Load<T>(Path.Join(ResourcesPath, pathName));

            if (_instCached)
                return _instCached;

            _instCached = CreateInstance<T>();

#if UNITY_EDITOR
            string path = Path.Join(BasePath, ResourcesPath, $"{pathName}.asset");
            Directory.CreateDirectory(Path.Join(Application.dataPath, BasePath, ResourcesPath));
            AssetDatabase.CreateAsset(_instCached, Path.Join("Assets", path));
            AssetDatabase.SaveAssets();
#endif

            return _instCached;
        }

#if UNITY_EDITOR

        protected static SettingsProvider GetSettings()
        {
            var attribute = typeof(T).GetCustomAttribute<SettingsObjectAttribute>();

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
            var attribute = typeof(T).GetCustomAttribute<SettingsObjectAttribute>();

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

                    scriptField?.RemoveFromHierarchy();

                    rootElement.Add(inspector);
                },
            };

            return provider;
        }
#endif
    }
}
