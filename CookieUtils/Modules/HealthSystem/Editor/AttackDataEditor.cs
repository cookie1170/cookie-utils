using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace CookieUtils.HealthSystem.Editor
{
    [CustomEditor(typeof(AttackData))]
    public class AttackDataEditor : UnityEditor.Editor
    {
        [SerializeField] private VisualTreeAsset inspector;

        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new();

            AttackData hitbox = (AttackData)target;

            inspector.CloneTree(root);

            Button editMask = root.Q<Button>("EditMask");
            PropertyField hasPierce = root.Q<PropertyField>("HasPierce");
            PropertyField destroyOnOutOfPierce = root.Q<PropertyField>("DestroyOnNoPierce");
            VisualElement hideIfNoPierce = root.Q<VisualElement>("HideIfNoPierce");
            VisualElement hideIfNoDestroy = root.Q<VisualElement>("HideIfNoDestroy");
            MaskField maskInput = root.Q<MaskField>("HitMask");

            UpdateChoices();

            maskInput.RegisterCallback<FocusEvent>(_ => UpdateChoices());

            editMask.RegisterCallback<ClickEvent>(_ => HealthSettings.OpenWindow());

            hasPierce.RegisterValueChangeCallback(_ =>
                hideIfNoPierce.style.display = hitbox.hasPierce ? DisplayStyle.Flex : DisplayStyle.None);

            destroyOnOutOfPierce.RegisterValueChangeCallback(_ =>
                hideIfNoDestroy.style.display = hitbox.destroyOnOutOfPierce ? DisplayStyle.Flex : DisplayStyle.None);

            return root;

            void UpdateChoices()
            {
                maskInput.choices = HealthSettings.Get().masks;
            }
        }
    }
}