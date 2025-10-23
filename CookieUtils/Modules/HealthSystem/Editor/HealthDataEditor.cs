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

        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new();

            HealthData health = (HealthData)target;

            inspector.CloneTree(root);

            PropertyField healCurve = root.Q<PropertyField>("RegenCurve");
            VisualElement destroyDelay = root.Q<VisualElement>("DestroyDelay");
            Button editMask = root.Q<Button>("EditMask");
            PropertyField hasRegen = root.Q<PropertyField>("HasRegen");
            PropertyField maxHealth = root.Q<PropertyField>("MaxHealth");
            PropertyField startHealth = root.Q<PropertyField>("StartHealth");
            Toggle destroyOnDeath = root.Q<Toggle>("DestroyOnDeath");
            MaskField maskInput = root.Q<MaskField>("HitMask");

            UpdateChoices();

            maskInput.RegisterCallback<FocusEvent>(_ => UpdateChoices());

            editMask.RegisterCallback<ClickEvent>(_ => HealthSettings.OpenWindow());

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

            void UpdateChoices()
            {
                maskInput.choices = HealthSettings.Get().masks;
            }
        }
    }
}