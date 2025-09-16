using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace CookieUtils.Health.Editor
{
    [CustomEditor(typeof(AttackData))]
    public class AttackDataEditor : UnityEditor.Editor
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

            var hitbox = (AttackData)target;

            inspector.CloneTree(root);
            
            
            var editMask = root.Q<Button>("EditMask");
            var hasPierce = root.Q<PropertyField>("HasPierce");
            var destroyOnOutOfPierce = root.Q<PropertyField>("DestroyOnNoPierce");
            var hideIfNoPierce = root.Q<VisualElement>("HideIfNoPierce");
            var hideIfNoDestroy = root.Q<VisualElement>("HideIfNoDestroy");
            _maskInput = root.Q<MaskField>("HitMask");
            
            UpdateMask(EditMask.GetMask().masks);
            
            editMask.RegisterCallback<ClickEvent>(_ => EditMask.ShowWindow());

            hasPierce.RegisterValueChangeCallback(_ =>
                hideIfNoPierce.style.display = hitbox.hasPierce ? DisplayStyle.Flex : DisplayStyle.None);
            
            destroyOnOutOfPierce.RegisterValueChangeCallback(_ =>
                hideIfNoDestroy.style.display = hitbox.destroyOnOutOfPierce ? DisplayStyle.Flex : DisplayStyle.None);
            
            return root;
        }
    }
}