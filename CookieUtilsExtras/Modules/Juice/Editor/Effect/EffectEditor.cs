using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace CookieUtils.Extras.Juice.Editor
{
    [CustomEditor(typeof(Effect))]
    public class EffectEditor : UnityEditor.Editor
    {
        [SerializeField] private VisualTreeAsset inspector;

        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new();

            inspector.CloneTree(root);

            Effect effect = (Effect)target;

            VisualElement dataObjectInspectorPanel = root.Q<VisualElement>("DataObjectInspectorPanel");
            Button createDataObject = root.Q<Button>("CreateDataObject");
            Foldout dataTitle = root.Q<Foldout>("DataTitle");
            PropertyField dataObject = root.Q<PropertyField>("DataObject");
            PropertyField overrideRenderer = root.Q<PropertyField>("OverrideRenderer");
            PropertyField rendererOverride = root.Q<PropertyField>("RendererOverride");
            PropertyField overrideMaterial = root.Q<PropertyField>("OverrideMaterial");
            PropertyField materialOverride = root.Q<PropertyField>("MaterialOverride");
            Button previewButton = root.Q<Button>("Preview");

            CheckDataObject();

            previewButton.SetEnabled(EditorApplication.isPlaying);

            previewButton.RegisterCallback<ClickEvent>(_ => effect.Play());

            dataObject.RegisterValueChangeCallback(_ => CheckDataObject());

            createDataObject.RegisterCallback<ClickEvent>(CreateDataObject);

            overrideRenderer.RegisterValueChangeCallback(_ =>
                rendererOverride.style.display = effect.overrideRenderers ? DisplayStyle.Flex : DisplayStyle.None);

            overrideMaterial.RegisterValueChangeCallback(_ =>
                materialOverride.style.display = effect.overrideMaterial ? DisplayStyle.Flex : DisplayStyle.None);

            return root;

            void CheckDataObject()
            {
                VisualElement dataInspectorCurrent = dataObjectInspectorPanel.Q<VisualElement>("DataInspector");
                if (dataInspectorCurrent != null) dataTitle.Remove(dataInspectorCurrent);

                if (effect.data) {
                    createDataObject.style.display = DisplayStyle.None;
                    InspectorElement dataInspector = new(effect.data) {
                        name = "DataInspector"
                    };

                    dataObjectInspectorPanel.style.display = DisplayStyle.Flex;
                    dataTitle.Add(dataInspector);
                    dataTitle.text = effect.data.name;
                } else {
                    createDataObject.style.display = DisplayStyle.Flex;
                    dataObjectInspectorPanel.style.display = DisplayStyle.None;
                }
            }

            void CreateDataObject(ClickEvent evt)
            {
                string path = EditorUtility.SaveFilePanelInProject("Create hurt effect  data",
                    $"{effect.name}_EffectData", "asset",
                    "Choose a path for the data object");

                EffectData data = CreateInstance<EffectData>();

                AssetDatabase.CreateAsset(data, path);
                Undo.RegisterCreatedObjectUndo(data, "Created effect data");
                Undo.RecordObject(effect, "Assigned effect data");
                effect.data = data;

                CheckDataObject();
            }
        }
    }
}