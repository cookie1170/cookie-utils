using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace CookieUtils.Base.Editor
{
    [PublicAPI]
    public struct SettingsWindowBuilder
    {
        private IEnumerable<string> _keywords;
        private SettingsScope _scopes;
        private Object _object;
        private string _path;
        private string _title;

        private SettingsWindowBuilder(Object obj)
        {
            _object = obj;
            _keywords = null;
            _scopes = SettingsScope.Project;
            _path = null;
            _title = null;
        }

        public static SettingsWindowBuilder Create(Object obj)
        {
            return new SettingsWindowBuilder(obj);
        }

        public SettingsWindowBuilder WithTitle(string title)
        {
            _title = title;
            return this;
        }

        public SettingsWindowBuilder WithPath(string path)
        {
            _path = path;
            return this;
        }

        public SettingsWindowBuilder WithScopes(SettingsScope scopes)
        {
            _scopes = scopes;
            return this;
        }

        public SettingsWindowBuilder WithKeywords(params string[] keywords)
        {
            _keywords = keywords;
            return this;
        }
        
        public SettingsProvider Build()
        {
            var obj = _object;
            string s = _title ?? _path.Split('/').Last();
            var provider = new SettingsProvider(_path, _scopes, _keywords) {
                label = _title,
                activateHandler = (_, rootElement) => {
                    var title = new Label(s) {
                        style = {
                            fontSize = 18,
                            paddingLeft = 16,
                            paddingTop = 2,
                            unityFontStyleAndWeight = FontStyle.Bold
                        }
                    };
                    rootElement.Add(title);

                    var inspector = new InspectorElement(obj);
                    
                    var scriptField = inspector.Query<PropertyField>()
                        .Where(f => f.bindingPath == "m_Script")
                        .First();
                    scriptField?.parent?.Remove(scriptField);

                    rootElement.Add(inspector);
                }
            };

            return provider;
        }
    }
}
