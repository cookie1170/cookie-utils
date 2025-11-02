using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CookieUtils.Debugging
{
    // ReSharper disable once InconsistentNaming
    internal class UGUIDebugUI_Builder : IDebugUIBuilder
    {
        private readonly DebugUI_Canvas _canvas;
        private readonly Stack<DebugUI_Group> _groups = new();
        private readonly DebugUI_Group _hostGroup;

        internal UGUIDebugUI_Builder(DebugUI_Canvas canvas, DebugUI_Group hostGroup) {
            _canvas = canvas;
            _hostGroup = hostGroup;
        }

        public IDebugUIBuilder Label(Func<string> updateText) {
            var label = Object.Instantiate<DebugUI_Label>(UGUIDebugUI_Helper.LabelPrefab);
            label.Init(updateText);
            Add(label);

            return this;
        }

        public IDebugUIBuilder FloatField(string text, Func<float> updateValue, Action<float> onValueEdited) {
            var field = Object.Instantiate<DebugUI_FloatField>(UGUIDebugUI_Helper.FloatFieldPrefab);
            field.Init(text, updateValue, onValueEdited);
            Add(field);

            return this;
        }

        public IDebugUIBuilder IntField(string text, Func<int> updateValue, Action<int> onValueEdited) {
            var field = Object.Instantiate<DebugUI_IntField>(UGUIDebugUI_Helper.IntFieldPrefab);
            field.Init(text, updateValue, onValueEdited);
            Add(field);

            return this;
        }

        public IDebugUIBuilder BoolField(string text, Func<bool> updateValue, Action<bool> onValueEdited) {
            var field = Object.Instantiate<DebugUI_BoolField>(UGUIDebugUI_Helper.BoolFieldPrefab);
            field.Init(text, updateValue, onValueEdited);
            Add(field);

            return this;
        }

        public IDebugUIBuilder StringField(string text, Func<string> updateValue, Action<string> onValueEdited) {
            var field = Object.Instantiate<DebugUI_StringField>(UGUIDebugUI_Helper.StringFieldPrefab);
            field.Init(text, updateValue, onValueEdited);
            Add(field);

            return this;
        }

        public IDebugUIBuilder Vector2Field(
            string text,
            Func<Vector2> updateValue,
            Action<Vector2> onValueEdited,
            string xLabel = "x",
            string yLabel = "y"
        ) {
            var field = Object.Instantiate<DebugUI_Vector2Field>(UGUIDebugUI_Helper.Vector2FieldPrefab);
            field.Init(text, updateValue, onValueEdited, xLabel, yLabel);
            Add(field);

            return this;
        }

        public IDebugUIBuilder Vector3Field(
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

            return this;
        }

        public IDebugUIBuilder Button(Func<string> updateText, Action onClicked) {
            var button = Object.Instantiate<DebugUI_Button>(UGUIDebugUI_Helper.ButtonPrefab);
            button.Init(updateText, onClicked);
            Add(button);

            return this;
        }

        public IDebugUIBuilder FoldoutGroup(Func<string> updateText, bool defaultShown) {
            var foldout = Object.Instantiate<DebugUI_FoldoutGroup>(UGUIDebugUI_Helper.FoldoutGroupPrefab);
            foldout.Init(updateText, defaultShown);
            Add(foldout);
            _groups.Push(foldout);

            return this;
        }

        public IDebugUIBuilder IfGroup(Func<bool> condition) {
            var group = Object.Instantiate<GameObject>(UGUIDebugUI_Helper.GroupPrefab).AddComponent<DebugUI_IfGroup>();
            group.Init(condition);
            Add(group);
            _groups.Push(group);

            return this;
        }

        public IDebugUIBuilder ElseGroup() {
            DebugUI_Group lastGroup = _groups.Pop();

            if (lastGroup is not DebugUI_IfGroup ifGroup)
                throw new ArgumentException(
                    "[CookieUtils.Debugging] An else group can only be started after an if group!"
                );

            var group = Object.Instantiate<GameObject>(UGUIDebugUI_Helper.GroupPrefab)
                .AddComponent<DebugUI_ElseGroup>();
            group.Init(ifGroup);
            Add(group);
            _groups.Push(group);

            return this;
        }

        public IDebugUIBuilder SwitchGroup<T>(Func<T> condition) {
            var group = Object.Instantiate<GameObject>(UGUIDebugUI_Helper.GroupPrefab)
                .AddComponent<DebugUI_SwitchGroup<T>>();
            group.Init(condition);
            Add(group);

            return this;
        }

        public IDebugUIBuilder CaseGroup<T>(Func<T> value) {
            var group = Object.Instantiate<GameObject>(UGUIDebugUI_Helper.GroupPrefab)
                .AddComponent<DebugUI_CaseGroup<T>>();
            group.Init(value);
            Add(group);

            return this;
        }

        public IDebugUIBuilder EndGroup() {
            _groups.TryPop(out _);

            return this;
        }

        private void Add(DebugUI_Element obj) {
            obj.OnMissingReference += () => _canvas.Clear();

            GetGroup().AddChild(obj.gameObject);
        }

        private DebugUI_Group GetGroup() => _groups.TryPeek(out DebugUI_Group group) ? group : _hostGroup;
    }
}