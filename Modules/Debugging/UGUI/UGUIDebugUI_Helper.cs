using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CookieUtils.Debugging
{
    // ReSharper disable once InconsistentNaming
    internal static class UGUIDebugUI_Helper
    {
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

        internal static readonly Prefab<DebugUI_Panel> PanelPrefab = "Panel";

        internal static DebugUI_Label InstantiateLabel(Func<string> updateText) {
            var label = Object.Instantiate<DebugUI_Label>(LabelPrefab);
            label.Init(updateText);

            return label;
        }

        internal static DebugUI_FloatField InstantiateFloatField(
            string text,
            Func<float> updateValue,
            Action<float> onValueEdited
        ) {
            var field = Object.Instantiate<DebugUI_FloatField>(FloatFieldPrefab);
            field.Init(text, updateValue, onValueEdited);

            return field;
        }

        internal static DebugUI_IntField InstantiateIntField(
            string text,
            Func<int> updateValue,
            Action<int> onValueEdited
        ) {
            var field = Object.Instantiate<DebugUI_IntField>(IntFieldPrefab);
            field.Init(text, updateValue, onValueEdited);

            return field;
        }

        internal static DebugUI_BoolField InstantiateBoolField(
            string text,
            Func<bool> updateValue,
            Action<bool> onValueEdited
        ) {
            var field = Object.Instantiate<DebugUI_BoolField>(BoolFieldPrefab);
            field.Init(text, updateValue, onValueEdited);

            return field;
        }

        internal static DebugUI_StringField InstantiateStringField(
            string text,
            Func<string> updateValue,
            Action<string> onValueEdited
        ) {
            var field = Object.Instantiate<DebugUI_StringField>(StringFieldPrefab);
            field.Init(text, updateValue, onValueEdited);

            return field;
        }

        internal static DebugUI_Vector2Field InstantiateVector2Field(
            string text,
            Func<Vector2> updateValue,
            Action<Vector2> onValueEdited,
            string xLabel,
            string yLabel
        ) {
            var field = Object.Instantiate<DebugUI_Vector2Field>(Vector2FieldPrefab);
            field.Init(text, updateValue, onValueEdited, xLabel, yLabel);

            return field;
        }

        internal static DebugUI_Vector3Field InstantiateVector3Field(
            string text,
            Func<Vector3> updateValue,
            Action<Vector3> onValueEdited,
            string xLabel,
            string yLabel,
            string zLabel
        ) {
            var field = Object.Instantiate<DebugUI_Vector3Field>(Vector3FieldPrefab);
            field.Init(text, updateValue, onValueEdited, xLabel, yLabel, zLabel);

            return field;
        }

        internal static DebugUI_Button InstantiateButton(Func<string> updateText, Action onClicked) {
            var button = Object.Instantiate<DebugUI_Button>(ButtonPrefab);
            button.Init(updateText, onClicked);

            return button;
        }

        internal static DebugUI_FoldoutGroup InstantiateFoldout(Func<string> updateText, bool defaultShown) {
            var foldout = Object.Instantiate<DebugUI_FoldoutGroup>(FoldoutGroupPrefab);
            foldout.Init(updateText, defaultShown);

            return foldout;
        }

        internal static DebugUI_IfGroup InstantiateIfGroup(Func<bool> condition) {
            var group = Object.Instantiate<GameObject>(GroupPrefab).AddComponent<DebugUI_IfGroup>();
            group.Init(condition);

            return group;
        }

        internal static DebugUI_ElseGroup InstantiateElseGroup(DebugUI_IfGroup ifGroup) {
            var group = Object.Instantiate<GameObject>(GroupPrefab).AddComponent<DebugUI_ElseGroup>();
            group.Init(ifGroup);

            return group;
        }

        internal static DebugUI_SwitchGroup<T> InstantiateSwitchGroup<T>(Func<T> condition) {
            var group = Object.Instantiate<GameObject>(GroupPrefab).AddComponent<DebugUI_SwitchGroup<T>>();
            group.Init(condition);

            return group;
        }

        internal static DebugUI_CaseGroup<T> InstantiateCaseGroup<T>(Func<T> value) {
            var group = Object.Instantiate<GameObject>(GroupPrefab).AddComponent<DebugUI_CaseGroup<T>>();
            group.Init(value);

            return group;
        }
    }
}