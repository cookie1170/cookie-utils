using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace CookieUtils.Extras.Juice.Editor
{
    [CustomEditor(typeof(EffectData))]
    public class EffectDataEditor : UnityEditor.Editor
    {
        [SerializeField] private VisualTreeAsset inspector;

        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new();

            inspector.CloneTree(root);

            EffectData data = (EffectData)target;

            PropertyField shakeCamera = root.Q<PropertyField>("ShakeCamera");
            PropertyField shakeForce = root.Q<PropertyField>("ShakeForce");
            PropertyField spawnParticles = root.Q<PropertyField>("SpawnParticles");
            VisualElement hideIfNoParticles = root.Q<VisualElement>("HideIfNoParticles");
            PropertyField playAudio = root.Q<PropertyField>("PlayAudio");
            VisualElement hideIfNoAudio = root.Q<VisualElement>("HideIfNoAudio");
            PropertyField animateScale = root.Q<PropertyField>("AnimateScale");
            PropertyField scaleSettings = root.Q<PropertyField>("ScaleSettings");
            PropertyField animateRotation = root.Q<PropertyField>("AnimateRotation");
            PropertyField rotationSettings = root.Q<PropertyField>("RotationSettings");
            PropertyField animateFlash = root.Q<PropertyField>("AnimateFlash");
            VisualElement hideIfNoFlash = root.Q<VisualElement>("HideIfNoFlash");

            shakeCamera.RegisterValueChangeCallback(_ =>
                shakeForce.style.display = data.shakeCamera ? DisplayStyle.Flex : DisplayStyle.None);

            spawnParticles.RegisterValueChangeCallback(_ =>
                hideIfNoParticles.style.display = data.spawnParticles ? DisplayStyle.Flex : DisplayStyle.None);

            playAudio.RegisterValueChangeCallback(_ =>
                hideIfNoAudio.style.display = data.playAudio ? DisplayStyle.Flex : DisplayStyle.None);

            animateScale.RegisterValueChangeCallback(_ =>
                scaleSettings.style.display = data.animateScale ? DisplayStyle.Flex : DisplayStyle.None);

            animateRotation.RegisterValueChangeCallback(_ =>
                rotationSettings.style.display = data.animateRotation ? DisplayStyle.Flex : DisplayStyle.None);

            animateFlash.RegisterValueChangeCallback(_ =>
                hideIfNoFlash.style.display = data.animateFlash ? DisplayStyle.Flex : DisplayStyle.None);

            return root;
        }
    }
}