using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace CookieUtils.HealthSystem.Editor
{
    [CustomEditor(typeof(Hitbox))]
    public class HitboxEditor : UnityEditor.Editor
    {
        [SerializeField] private VisualTreeAsset inspector;

        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new();

            Hitbox hitbox = (Hitbox)target;

            inspector.CloneTree(root);

            Button createDataObject = root.Q<Button>("GenerateDataObject");
            VisualElement dataObjectInspectorPanel = root.Q<VisualElement>("DataObjectInspectorPanel");
            Foldout dataTitle = root.Q<Foldout>("DataTitle");
            PropertyField dataObject = root.Q<PropertyField>("DataObject");

            createDataObject.RegisterCallback<ClickEvent>(CreateDataObject);

            dataObject.RegisterValueChangeCallback(_ => CheckDataObject());

            return root;

            void CheckDataObject()
            {
                VisualElement dataInspectorCurrent = dataObjectInspectorPanel.Q<VisualElement>("DataInspector");
                if (dataInspectorCurrent != null) dataTitle.Remove(dataInspectorCurrent);

                if (hitbox.data) {
                    createDataObject.style.display = DisplayStyle.None;
                    InspectorElement dataInspector = new(hitbox.data) {
                        name = "DataInspector"
                    };

                    dataObjectInspectorPanel.style.display = DisplayStyle.Flex;
                    dataTitle.Add(dataInspector);
                    dataTitle.text = hitbox.data.name;
                } else {
                    createDataObject.style.display = DisplayStyle.Flex;
                    dataObjectInspectorPanel.style.display = DisplayStyle.None;
                }
            }

            void CreateDataObject(ClickEvent evt)
            {
                string path = EditorUtility.SaveFilePanelInProject("Create attack data",
                    $"{(hitbox.transform.parent ? hitbox.transform.parent.name : hitbox.name)}_AttackData", "asset",
                    "Choose a path for the data object");

                AttackData data = CreateInstance<AttackData>();

                AssetDatabase.CreateAsset(data, path);
                Undo.RegisterCreatedObjectUndo(data, "Created attack data");
                Undo.RecordObject(hitbox, "Assigned attack data");
                hitbox.data = data;

                CheckDataObject();
            }
        }
    }
}