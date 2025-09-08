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

        //TODO: Make the create data object button work
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
            _maskInput = root.Q<MaskField>("HitMask");

            UpdateMask(EditMask.GetMask().masks);

            editMask.RegisterCallback<ClickEvent>(_ => EditMask.ShowWindow());
            
            root.Q<PropertyField>("HasRegen").RegisterValueChangeCallback(_ =>
                healCurve.style.display = health.hasRegen ? DisplayStyle.Flex : DisplayStyle.None);

            root.Q<PropertyField>("MaxHealth").RegisterValueChangeCallback(_ => ClampStartHealth());

            root.Q<PropertyField>("StartHealth").RegisterValueChangeCallback(_ => ClampStartHealth());

            root.Q<Toggle>("DestroyOnDeath").RegisterValueChangedCallback(_ => {
                destroyDelay.style.display =
                    health.destroyOnDeath ? DisplayStyle.Flex : DisplayStyle.None;
            });

            root.Q<PropertyField>("UseDataObject").RegisterValueChangeCallback(_ => {
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
                hideOnUseDataObject.style.display =
                    health.useDataObject && health.data ? DisplayStyle.None : DisplayStyle.Flex;
            }
        }
    }
}
