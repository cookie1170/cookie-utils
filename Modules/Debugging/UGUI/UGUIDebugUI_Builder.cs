using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CookieUtils.Debugging
{
    // ReSharper disable once InconsistentNaming
    internal class UGUIDebugUI_Builder : IDebugUI_If, IDebugUI_Switch
    {
        private readonly DebugUI_Canvas _canvas;
        private readonly DebugUI_Group _hostGroup;

        internal UGUIDebugUI_Builder(DebugUI_Canvas canvas, DebugUI_Group hostGroup) {
            _canvas = canvas;
            _hostGroup = hostGroup;
        }

        public void Label(Func<string> updateText) {
            var label = Object.Instantiate<DebugUI_Label>(UGUIDebugUI_Helper.LabelPrefab);
            label.Init(updateText);
            Add(label);
        }

        public void FloatField(string text, Func<float> updateValue, Action<float> onValueEdited) {
            var field = Object.Instantiate<DebugUI_FloatField>(UGUIDebugUI_Helper.FloatFieldPrefab);
            field.Init(text, updateValue, onValueEdited);
            Add(field);
        }

        public void IntField(string text, Func<int> updateValue, Action<int> onValueEdited) {
            var field = Object.Instantiate<DebugUI_IntField>(UGUIDebugUI_Helper.IntFieldPrefab);
            field.Init(text, updateValue, onValueEdited);
            Add(field);
        }

        public void BoolField(string text, Func<bool> updateValue, Action<bool> onValueEdited) {
            var field = Object.Instantiate<DebugUI_BoolField>(UGUIDebugUI_Helper.BoolFieldPrefab);
            field.Init(text, updateValue, onValueEdited);
            Add(field);
        }

        public void StringField(string text, Func<string> updateValue, Action<string> onValueEdited) {
            var field = Object.Instantiate<DebugUI_StringField>(UGUIDebugUI_Helper.StringFieldPrefab);
            field.Init(text, updateValue, onValueEdited);
            Add(field);
        }

        public void Vector2Field(
            string text,
            Func<Vector2> updateValue,
            Action<Vector2> onValueEdited,
            string xLabel = "x",
            string yLabel = "y"
        ) {
            var field = Object.Instantiate<DebugUI_Vector2Field>(UGUIDebugUI_Helper.Vector2FieldPrefab);
            field.Init(text, updateValue, onValueEdited, xLabel, yLabel);
            Add(field);
        }

        public void Vector3Field(
            string text,
            Func<Vector3> updateValue,
            Action<Vector3> onValueEdited,
            string xLabel = "x",
            string yLabel = "y",
            string zLabel = "z"
        ) {
            var field = Object.Instantiate<DebugUI_Vector3Field>(UGUIDebugUI_Helper.Vector3FieldPrefab);
            field.Init(text, updateValue, onValueEdited, xLabel, yLabel, zLabel);
            Add(field);
        }

        public void Button(Func<string> updateText, Action onClicked) {
            var button = Object.Instantiate<DebugUI_Button>(UGUIDebugUI_Helper.ButtonPrefab);
            button.Init(updateText, onClicked);
            Add(button);
        }

        public IDebugUI_Group FoldoutGroup(Func<string> updateText, bool defaultShown) {
            var foldout = Object.Instantiate<DebugUI_FoldoutGroup>(UGUIDebugUI_Helper.FoldoutGroupPrefab);
            foldout.Init(updateText, defaultShown);
            Add(foldout);

            return new UGUIDebugUI_Builder(_canvas, foldout);
        }

        public IDebugUI_If IfGroup(Func<bool> condition) {
            var group = Object.Instantiate<GameObject>(UGUIDebugUI_Helper.GroupPrefab).AddComponent<DebugUI_IfGroup>();
            group.Init(condition);
            Add(group);

            return new UGUIDebugUI_Builder(_canvas, group);
        }

        public IDebugUI_Group ElseGroup() {
            var group = Object.Instantiate<GameObject>(UGUIDebugUI_Helper.GroupPrefab)
                .AddComponent<DebugUI_ElseGroup>();
            Add(group);

            return new UGUIDebugUI_Builder(_canvas, group);
        }

        public IDebugUI_Switch SwitchGroup<T>(Func<T> condition) {
            var group = Object.Instantiate<GameObject>(UGUIDebugUI_Helper.GroupPrefab)
                .AddComponent<DebugUI_SwitchGroup<T>>();
            group.Init(condition);
            Add(group);

            return new UGUIDebugUI_Builder(_canvas, group);
        }

        public IDebugUI_Group CaseGroup<T>(Func<T> value) {
            var group = Object.Instantiate<GameObject>(UGUIDebugUI_Helper.GroupPrefab)
                .AddComponent<DebugUI_CaseGroup<T>>();
            group.Init(value);
            Add(group);

            return new UGUIDebugUI_Builder(_canvas, group);
        }

        private void Add(DebugUI_Element obj) {
            obj.OnMissingReference += () => _canvas.Clear();

            _hostGroup.AddChild(obj.gameObject);
        }
    }
}