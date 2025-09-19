using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace CookieUtils.Extras.Scenes.Editor
{
    public class ScenesSettingsWindow : EditorWindow
    {
        [MenuItem("Cookie Utils/Scenes/Scene groups")]
        public static void CreateWindow()
        {
            GetWindow<ScenesSettingsWindow>("Scenes");
        }

        private void CreateGUI()
        {
            var data = ScenesData.GetScenesData();

            var dataInspector = new InspectorElement(data);
            var scriptField = dataInspector.Q<PropertyField>("PropertyField:m_Script");
            scriptField.parent.Remove(scriptField);
            
            rootVisualElement.Add(dataInspector);
        }
    }
}