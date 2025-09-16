using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace CookieUtils.Health.Editor
{
    [CustomEditor(typeof(Hitbox2D))]
    public class Hitbox2DEditor : HitboxEditor
    {
        public override VisualElement CreateInspectorGUI()
        {
            var root = base.CreateInspectorGUI();

            var hitbox2D = (Hitbox2D)target;
            
            var panel2D = new VisualElement();
            panel2D.AddToClassList("panel");
            
            var title2D = new Foldout {
                text = "2D",
                viewDataKey = "title2D"
            };

            title2D.AddToClassList("title");

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
                triggerOverride.style.display = hitbox2D.overrideTrigger ? DisplayStyle.Flex : DisplayStyle.None);

            title2D.Add(overrideTrigger);
            title2D.Add(triggerOverride);

            panel2D.Add(title2D);

            root.Add(panel2D);

            return root;
        }
    }
}