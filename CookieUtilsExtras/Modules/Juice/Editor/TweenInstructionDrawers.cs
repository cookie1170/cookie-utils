using UnityEditor;
using UnityEditor.Graphs;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace CookieUtils.Extras.Effect.Editor
{
    [CustomPropertyDrawer(typeof(TransformTweenInstruction))]
    public class TransformTweenInstructionDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var root = new VisualElement();

            var foldout = new Foldout {
                text = property.displayName,
            };
            var parallelField = new PropertyField(property.FindPropertyRelative("parallel"));
            var typeField = new PropertyField(property.FindPropertyRelative("type"));
            var normalSettings = new PropertyField(property.FindPropertyRelative("settings"), "Settings");
            var shakeSettings = new PropertyField(property.FindPropertyRelative("shakeSettings"), "Settings");

            CheckType();
            typeField.RegisterValueChangeCallback(_ => CheckType());
            
            foldout.Add(parallelField);
            foldout.Add(typeField);
            foldout.Add(normalSettings);
            foldout.Add(shakeSettings);
            root.Add(foldout);
            return root;

            void CheckType()
            {
                var type = (TweenType)property.FindPropertyRelative("type").enumValueIndex;
                if (type is TweenType.Punch or TweenType.Shake) {
                    shakeSettings.style.display = DisplayStyle.Flex;
                    normalSettings.style.display = DisplayStyle.None;
                } else {
                    shakeSettings.style.display = DisplayStyle.None;
                    normalSettings.style.display = DisplayStyle.Flex;
                }
            }
        }
    }

    [CustomPropertyDrawer(typeof(ScaleTweenInstruction))]
    public class ScaleTweenInstructionPropertyDrawer : TransformTweenInstructionDrawer
    {
    }

    [CustomPropertyDrawer(typeof(RotationTweenInstruction))]
    public class RotationTweenInstructionPropertyDrawer : TransformTweenInstructionDrawer
    {
    }
}