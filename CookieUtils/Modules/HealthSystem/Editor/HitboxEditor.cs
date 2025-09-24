using System.Collections.Generic;
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

            var hitbox = (Hitbox)target;

            inspector.CloneTree(root);
            
            var useDataObject = root.Q<PropertyField>("UseDataObject");
            var createDataObject = root.Q<Button>("GenerateDataObject");
            var dataObjectInspectorPanel = root.Q<VisualElement>("DataObjectInspectorPanel");
            var dataTitle = root.Q<Foldout>("DataTitle");
            var dataObject = root.Q<PropertyField>("DataObject");
            var hideOnUseDataObject = root.Q<VisualElement>("HideOnUseDataObject");
            var editMask = root.Q<Button>("EditMask");
            var hasPierce = root.Q<PropertyField>("HasPierce");
            var destroyOnOutOfPierce = root.Q<PropertyField>("DestroyOnNoPierce");
            var hideIfNoPierce = root.Q<VisualElement>("HideIfNoPierce");
            var hideIfNoDestroy = root.Q<VisualElement>("HideIfNoDestroy");
            _maskInput = root.Q<MaskField>("HitMask");

            UpdateMask(EditMask.GetMask().masks);
            
            createDataObject.RegisterCallback<ClickEvent>(CreateDataObject);
            
            useDataObject.RegisterValueChangeCallback(_ => {
                dataObject.style.display =
                    hitbox.useDataObject ? DisplayStyle.Flex : DisplayStyle.None;
                CheckDataObject();
            });

            dataObject.RegisterValueChangeCallback(_ => CheckDataObject());
            
            editMask.RegisterCallback<ClickEvent>(_ => EditMask.ShowWindow());

            hasPierce.RegisterValueChangeCallback(_ =>
                hideIfNoPierce.style.display = hitbox.hasPierce ? DisplayStyle.Flex : DisplayStyle.None);
            
            destroyOnOutOfPierce.RegisterValueChangeCallback(_ =>
                hideIfNoDestroy.style.display = hitbox.destroyOnOutOfPierce ? DisplayStyle.Flex : DisplayStyle.None);
            
            return root;
            
            void CheckDataObject()
            {
                var dataInspectorCurrent = dataObjectInspectorPanel.Q<VisualElement>("DataInspector");
                if (dataInspectorCurrent != null) dataTitle.Remove(dataInspectorCurrent);
               
                if (hitbox.useDataObject && hitbox.data) {
                    hideOnUseDataObject.style.display = DisplayStyle.None;
                    createDataObject.style.display = DisplayStyle.None;
                    var dataInspector = new InspectorElement(hitbox.data) {
                        name = "DataInspector"
                    };
                    
                    dataInspector.Query<PropertyField>()
                        .ForEach(f => f.RegisterValueChangeCallback(_ => hitbox.UpdateData()));
                    
                    dataObjectInspectorPanel.style.display = DisplayStyle.Flex;
                    dataTitle.Add(dataInspector);
                    dataTitle.text = hitbox.data.name;
                } else {
                    createDataObject.style.display = DisplayStyle.Flex;
                    dataObjectInspectorPanel.style.display = DisplayStyle.None;
                    hideOnUseDataObject.style.display = DisplayStyle.Flex;
                }
            }
            
            void CreateDataObject(ClickEvent evt)
            {
                string path = EditorUtility.SaveFilePanelInProject("Create attack data",
                    $"{(hitbox.transform.parent ? hitbox.transform.parent.name : hitbox.name)}_AttackData", "asset",
                    "Choose a path for the data object");

                var data = CreateHealthData();
                
                AssetDatabase.CreateAsset(data, path);
                hitbox.useDataObject = true;
                hitbox.data = data;
                
                CheckDataObject();
            }

            AttackData CreateHealthData()
            {
                var data = CreateInstance<AttackData>();
                data.destroyOnOutOfPierce = hitbox.destroyOnOutOfPierce;
                data.destroyParent = hitbox.destroyParent;
                data.directionType = hitbox.directionType;
                data.destroyDelay = hitbox.destroyDelay;
                data.hasPierce = hitbox.hasPierce;
                data.iframes = hitbox.iframes;
                data.damage = hitbox.damage;
                data.pierce = hitbox.pierce;
                data.mask = hitbox.mask;

                return data;
            }
        }
    }
}