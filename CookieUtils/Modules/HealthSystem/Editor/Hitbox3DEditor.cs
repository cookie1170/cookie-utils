using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace CookieUtils.HealthSystem.Editor
{
    [CustomEditor(typeof(Hitbox3D))]
    public class Hitbox3DEditor : HitboxEditor
    {
        public override VisualElement CreateInspectorGUI()
        {
            var root = base.CreateInspectorGUI();

            var hitbox3D = (Hitbox3D)target;
            
            var panel3D = new VisualElement();
            panel3D.AddToClassList("panel");
            
            var title3D = new Foldout {
                text = "3D",
                viewDataKey = "title3D"
            };

            title3D.AddToClassList("title");

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
                triggerOverride.style.display = hitbox3D.overrideTrigger ? DisplayStyle.Flex : DisplayStyle.None);

            title3D.Add(overrideTrigger);
            title3D.Add(triggerOverride);

            panel3D.Add(title3D);

            root.Add(panel3D);

            return root;
        }
    }
}