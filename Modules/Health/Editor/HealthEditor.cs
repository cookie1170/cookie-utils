using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace CookieUtils.Health.Editor
{
    [CustomEditor(typeof(Health))]
    public class HealthEditor : UnityEditor.Editor
    {
        [SerializeField] private VisualTreeAsset inspector;
        private MaskField _maskInput;

        private void OnEnable()
        {
            EditMask.OnMaskChanged += UpdateMask;
        }

        private void OnDisable()
        {
            EditMask.OnMaskChanged -= UpdateMask;
        }

        private void UpdateMask(List<string> masks)
        {
            if (_maskInput != null) {
                _maskInput.choices = masks;
            }
        }
        
        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();

            var health = (Health)target;
            
            inspector.CloneTree(root);

            var healCurve = root.Q<CurveField>("RegenCurve");
            var destroyDelay = root.Q<VisualElement>("DestroyDelay");
            var dataObject = root.Q<ObjectField>("DataObject");
            var hideOnUseDataObject = root.Q<VisualElement>("HideOnUseDataObject");
            var editMask = root.Q<Button>("EditMask");
            var createDataObject = root.Q<Button>("GenerateDataObject");
            var hasRegen = root.Q<PropertyField>("HasRegen");
            var maxHealth = root.Q<PropertyField>("MaxHealth");
            var startHealth = root.Q<PropertyField>("StartHealth");
            var destroyOnDeath = root.Q<Toggle>("DestroyOnDeath");
            var useDataObject = root.Q<PropertyField>("UseDataObject");
            var dataObjectInspectorPanel = root.Q<VisualElement>("DataObjectInspectorPanel");
            var dataTitle = root.Q<Label>("DataTitle");
            _maskInput = root.Q<MaskField>("HitMask");

            createDataObject.RegisterCallback<ClickEvent>(CreateDataObject);

            UpdateMask(EditMask.GetMask().masks);

            editMask.RegisterCallback<ClickEvent>(_ => EditMask.ShowWindow());

            hasRegen.RegisterValueChangeCallback(_ =>
                healCurve.style.display = health.hasRegen ? DisplayStyle.Flex : DisplayStyle.None);

            maxHealth.RegisterValueChangeCallback(_ => ClampStartHealth());

            startHealth.RegisterValueChangeCallback(_ => ClampStartHealth());

            destroyOnDeath.RegisterValueChangedCallback(_ => {
                destroyDelay.style.display =
                    health.destroyOnDeath ? DisplayStyle.Flex : DisplayStyle.None;
            });

            useDataObject.RegisterValueChangeCallback(_ => {
                dataObject.style.display =
                    health.useDataObject ? DisplayStyle.Flex : DisplayStyle.None;
                CheckDataObject();
            });

            dataObject.RegisterValueChangedCallback(_ => CheckDataObject());

            return root;

            void ClampStartHealth()
            {
                health.startHealth = Mathf.Clamp(health.startHealth, 1, health.maxHealth);
            }

            void CheckDataObject()
            {
                var dataInspectorCurrent = dataObjectInspectorPanel.Q<VisualElement>("DataInspector");
                if (dataInspectorCurrent != null) dataObjectInspectorPanel.Remove(dataInspectorCurrent);
               
                if (health.useDataObject && health.data) {
                    hideOnUseDataObject.style.display = DisplayStyle.None;
                    createDataObject.style.display = DisplayStyle.None;
                    var dataInspector = new InspectorElement(health.data) {
                        name = "DataInspector"
                    };
                    dataObjectInspectorPanel.style.display = DisplayStyle.Flex;
                    dataObjectInspectorPanel.Add(dataInspector);
                    dataTitle.text = health.data.name;
                } else {
                    createDataObject.style.display = DisplayStyle.Flex;
                    dataObjectInspectorPanel.style.display = DisplayStyle.None;
                    hideOnUseDataObject.style.display = DisplayStyle.Flex;
                }
            }
            
            void CreateDataObject(ClickEvent evt)
            {
                string path = EditorUtility.SaveFilePanelInProject("Create health data", $"{health.name}_HealthData", "asset",
                    "Choose a path for the data object");

                var data = CreateHealthData();
                
                AssetDatabase.CreateAsset(data, path);
                health.useDataObject = true;
                health.data = data;
                
                CheckDataObject();
            }

            HealthData CreateHealthData()
            {
                var data = CreateInstance<HealthData>();
                data.destroyOnDeath = health.destroyOnDeath;
                data.destroyDelay = health.destroyDelay;
                data.startHealth = health.startHealth;
                data.iframeType = health.iframeType;
                data.iframeMult = health.iframeMult;
                data.regenCurve = health.regenCurve;
                data.maxHealth = health.maxHealth;
                data.hasRegen = health.hasRegen;
                data.mask = health.mask;

                return data;
            }
        }
    }
}
