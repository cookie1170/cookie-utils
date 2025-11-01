using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CookieUtils.Debugging
{
    // ReSharper disable once InconsistentNaming
    internal class DebugUI_Panel : MonoBehaviour
    {
        [SerializeField] private Transform content;
        [SerializeField] private Image lockIcon;
        [SerializeField] private Sprite lockedSprite;
        [SerializeField] private Sprite unlockedSprite;
        [SerializeField] internal Button lockButton;

        private readonly Stack<DebugUI_Group> _groups = new();
        private DebugUI_Canvas _canvas;
        private Image _imageCached;

        internal Image Image {
            get {
                if (_imageCached) return _imageCached;

                _imageCached = GetComponent<Image>();

                return _imageCached;
            }
        }

        internal void SetLocked(bool value) {
            lockIcon.sprite = value ? lockedSprite : unlockedSprite;
        }

        internal void Init(DebugUI_Canvas canvas) {
            _canvas = canvas;
        }

        private void Add(DebugUI_Element obj) {
            if (!_groups.TryPeek(out DebugUI_Group group)) {
                obj.transform.SetParent(content, false);

                return;
            }

            obj.OnMissingReference += () => _canvas.Clear();

            group.AddChild(obj.gameObject);
        }

        internal void Label(Func<string> updateText) {
            var label = Instantiate<DebugUI_Label>(LabelPrefab);
            Add(label);
            label.Init(updateText);
        }

        internal void FloatField(string text, Func<float> updateValue, Action<float> onValueEdited) {
            var field = Instantiate<DebugUI_FloatField>(FloatFieldPrefab);
            Add(field);
            field.Init(text, updateValue, onValueEdited);
        }

        internal void IntField(string text, Func<int> updateValue, Action<int> onValueEdited) {
            var field = Instantiate<DebugUI_IntField>(IntFieldPrefab);
            Add(field);
            field.Init(text, updateValue, onValueEdited);
        }

        internal void BoolField(string text, Func<bool> updateValue, Action<bool> onValueEdited) {
            var field = Instantiate<DebugUI_BoolField>(BoolFieldPrefab);
            Add(field);
            field.Init(text, updateValue, onValueEdited);
        }

        internal void StringField(string text, Func<string> updateValue, Action<string> onValueEdited) {
            var field = Instantiate<DebugUI_StringField>(StringFieldPrefab);
            Add(field);
            field.Init(text, updateValue, onValueEdited);
        }

        internal void Vector2Field(
            string text,
            Func<Vector2> updateValue,
            Action<Vector2> onValueEdited,
            string xLabel,
            string yLabel
        ) {
            var field = Instantiate<DebugUI_Vector2Field>(Vector2FieldPrefab);
            Add(field);
            field.Init(text, updateValue, onValueEdited, xLabel, yLabel);
        }

        internal void Vector3Field(
            string text,
            Func<Vector3> updateValue,
            Action<Vector3> onValueEdited,
            string xLabel,
            string yLabel,
            string zLabel
        ) {
            var field = Instantiate<DebugUI_Vector3Field>(Vector3FieldPrefab);
            Add(field);
            field.Init(text, updateValue, onValueEdited, xLabel, yLabel, zLabel);
        }

        internal void Button(Func<string> updateText, Action onClicked) {
            var button = Instantiate<DebugUI_Button>(ButtonPrefab);
            Add(button);
            button.Init(updateText, onClicked);
        }


        internal void FoldoutGroup(Func<string> updateText, bool defaultShown) {
            var foldout = Instantiate<DebugUI_FoldoutGroup>(FoldoutGroupPrefab);
            Add(foldout);
            foldout.Init(updateText, defaultShown);
            _groups.Push(foldout);
        }

        internal void IfGroup(Func<bool> condition) {
            var group = Instantiate<GameObject>(GroupPrefab).AddComponent<DebugUI_IfGroup>();
            Add(group);
            group.Init(condition);
            _groups.Push(group);
        }

        internal void ElseGroup() {
            DebugUI_Group lastGroup = _groups.Pop();

            if (lastGroup is not DebugUI_IfGroup ifGroup)
                throw new ArgumentException(
                    "[CookieUtils.Debugging] An else group can only be started after an if group!"
                );

            var group = Instantiate<GameObject>(GroupPrefab).AddComponent<DebugUI_ElseGroup>();
            Add(group);
            group.Init(ifGroup);
            _groups.Push(group);
        }

        internal void SwitchGroup<T>(Func<T> condition) {
            var group = Instantiate<GameObject>(GroupPrefab).AddComponent<DebugUI_SwitchGroup<T>>();
            Add(group);
            group.Init(condition);
        }

        internal void CaseGroup<T>(Func<T> value) {
            var group = Instantiate<GameObject>(GroupPrefab).AddComponent<DebugUI_CaseGroup<T>>();
            Add(group);
            group.Init(value);
        }

        internal void EndGroup() {
            _groups.TryPop(out _);
        }

        #region Prefabs

        private static readonly Prefab<DebugUI_FoldoutGroup> FoldoutGroupPrefab = "FoldoutGroup";
        private static readonly Prefab<DebugUI_Label> LabelPrefab = "Label";
        private static readonly Prefab<GameObject> GroupPrefab = "Group";
        private static readonly Prefab<DebugUI_Button> ButtonPrefab = "Button";
        private static readonly Prefab<DebugUI_FloatField> FloatFieldPrefab = "Fields/FloatField";
        private static readonly Prefab<DebugUI_IntField> IntFieldPrefab = "Fields/IntField";
        private static readonly Prefab<DebugUI_BoolField> BoolFieldPrefab = "Fields/BoolField";
        private static readonly Prefab<DebugUI_StringField> StringFieldPrefab = "Fields/StringField";
        private static readonly Prefab<DebugUI_Vector2Field> Vector2FieldPrefab = "Fields/Vector2Field";
        private static readonly Prefab<DebugUI_Vector3Field> Vector3FieldPrefab = "Fields/Vector3Field";

        #endregion
    }
}