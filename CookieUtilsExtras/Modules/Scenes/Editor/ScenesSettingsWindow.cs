using UnityEditor;
using UnityEditor.UIElements;

namespace CookieUtils.Extras.Scenes.Editor
{
    public class ScenesSettingsWindow : EditorWindow
    {
        [MenuItem("Cookie Utils/Scenes/Scene groups")]
        private static void CreateWindow()
        {
            GetWindow<ScenesSettingsWindow>("Scenes");
        }

        private void CreateGUI()
        {
            var data = ScenesData.GetScenesData();

            var dataInspector = new InspectorElement(data);

            rootVisualElement.Add(dataInspector);
        }
    }
}