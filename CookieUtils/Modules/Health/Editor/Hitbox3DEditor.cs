using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace CookieUtils.Health.Editor
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
            
            var overrideRigidbody = new PropertyField {
                bindingPath = "overrideRigidbody",
                label = "Override rigidbody"
            };
            
            var rigidbodyOverride = new ObjectField {
                bindingPath = "rigidbody",
                objectType = typeof(Rigidbody),
                label = "Rigidbody"
            };
            
            root.Q<PropertyField>("DirectionType").RegisterValueChangeCallback(_ => {
                if (hitbox3D.directionType == Hitbox.DirectionTypes.Rigidbody) {
                    overrideRigidbody.style.display = DisplayStyle.Flex;
                    if (hitbox3D.overrideRigidbody) rigidbodyOverride.style.display = DisplayStyle.Flex;
                } else {
                    overrideRigidbody.style.display = DisplayStyle.None;
                    rigidbodyOverride.style.display = DisplayStyle.None;
                }
            });

            overrideRigidbody.RegisterValueChangeCallback(_ =>
                rigidbodyOverride.style.display =
                    hitbox3D.overrideRigidbody && hitbox3D.directionType == Hitbox.DirectionTypes.Rigidbody
                        ? DisplayStyle.Flex
                        : DisplayStyle.None);

            title3D.Add(overrideRigidbody);
            title3D.Add(rigidbodyOverride);

            panel3D.Add(title3D);

            root.Add(panel3D);

            return root;
        }
    }
}