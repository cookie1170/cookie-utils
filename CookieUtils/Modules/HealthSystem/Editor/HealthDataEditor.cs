using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace CookieUtils.HealthSystem.Editor
{
    [CustomEditor(typeof(HealthData))]
    public class HealthDataEditor : UnityEditor.Editor
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

            var health = (HealthData)target;

            inspector.CloneTree(root);

            var healCurve = root.Q<PropertyField>("RegenCurve");
            var destroyDelay = root.Q<VisualElement>("DestroyDelay");
            var editMask = root.Q<Button>("EditMask");
            var hasRegen = root.Q<PropertyField>("HasRegen");
            var maxHealth = root.Q<PropertyField>("MaxHealth");
            var startHealth = root.Q<PropertyField>("StartHealth");
            var destroyOnDeath = root.Q<Toggle>("DestroyOnDeath");
            _maskInput = root.Q<MaskField>("HitMask");

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

            return root;

            void ClampStartHealth()
            {
                health.startHealth = Mathf.Clamp(health.startHealth, 1, health.maxHealth);
            }
        }
    }
}
