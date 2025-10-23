using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace CookieUtils.HealthSystem.Editor
{
    [CustomEditor(typeof(Hurtbox2D))]
    public class Hurtbox2DEditor : UnityEditor.Editor
    {
        [SerializeField] private StyleSheet style;
        
        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();

            root.styleSheets.Add(style);
                
            var hurtbox2D = (Hurtbox2D)target;
            
            var panel2D = new VisualElement();
            panel2D.AddToClassList("panel");

            var overrideTrigger = new PropertyField {
                bindingPath = "overrideTrigger",
                label = "Override trigger"
            };
            
            var triggerOverride = new ObjectField {
                bindingPath = "trigger",
                objectType = typeof(Collider2D),
                label = "Trigger"
            };

            overrideTrigger.RegisterValueChangeCallback(_ =>
                triggerOverride.style.display = hurtbox2D.overrideTrigger ? DisplayStyle.Flex : DisplayStyle.None);

            panel2D.Add(overrideTrigger);
            panel2D.Add(triggerOverride);

            root.Add(panel2D);

            return root;
        }
    }
}