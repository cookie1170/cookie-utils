using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace CookieUtils.HealthSystem.Editor
{
    [CustomEditor(typeof(Hurtbox3D))]
    public class Hurtbox3DEditor : UnityEditor.Editor
    {
        [SerializeField] private StyleSheet style;
        
        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();
            
            root.styleSheets.Add(style);

            var hurtbox3D = (Hurtbox3D)target;
            
            var panel3D = new VisualElement();
            panel3D.AddToClassList("panel");

            var overrideTrigger = new PropertyField {
                bindingPath = "overrideTrigger",
                label = "Override trigger"
            };
            
            var triggerOverride = new ObjectField {
                bindingPath = "trigger",
                objectType = typeof(Collider),
                label = "Trigger"
            };

            overrideTrigger.RegisterValueChangeCallback(_ =>
                triggerOverride.style.display = hurtbox3D.overrideTrigger ? DisplayStyle.Flex : DisplayStyle.None);

            panel3D.Add(overrideTrigger);
            panel3D.Add(triggerOverride);

            root.Add(panel3D);

            return root;
        }
    }
}