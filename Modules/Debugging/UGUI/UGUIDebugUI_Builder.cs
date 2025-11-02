using System;
using System.Collections.Generic;
using UnityEngine;

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
            DebugUI_Label label = UGUIDebugUI_Helper.InstantiateLabel(updateText);
            Add(label);

            return this;
        }

        public IDebugUIBuilder FloatField(string text, Func<float> updateValue, Action<float> onValueEdited) {
            DebugUI_FloatField field = UGUIDebugUI_Helper.InstantiateFloatField(text, updateValue, onValueEdited);
            Add(field);

            return this;
        }

        public IDebugUIBuilder IntField(string text, Func<int> updateValue, Action<int> onValueEdited) {
            DebugUI_IntField field = UGUIDebugUI_Helper.InstantiateIntField(text, updateValue, onValueEdited);
            Add(field);

            return this;
        }

        public IDebugUIBuilder BoolField(string text, Func<bool> updateValue, Action<bool> onValueEdited) {
            DebugUI_BoolField field = UGUIDebugUI_Helper.InstantiateBoolField(text, updateValue, onValueEdited);
            Add(field);

            return this;
        }

        public IDebugUIBuilder StringField(string text, Func<string> updateValue, Action<string> onValueEdited) {
            DebugUI_StringField field = UGUIDebugUI_Helper.InstantiateStringField(text, updateValue, onValueEdited);
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
            DebugUI_Vector2Field field = UGUIDebugUI_Helper.InstantiateVector2Field(
                text, updateValue, onValueEdited, xLabel, yLabel
            );
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
            DebugUI_Vector3Field field = UGUIDebugUI_Helper.InstantiateVector3Field(
                text, updateValue, onValueEdited, xLabel, yLabel, zLabel
            );
            Add(field);

            return this;
        }

        public IDebugUIBuilder Button(Func<string> updateText, Action onClicked) {
            DebugUI_Button button = UGUIDebugUI_Helper.InstantiateButton(updateText, onClicked);
            Add(button);

            return this;
        }

        public IDebugUIBuilder FoldoutGroup(Func<string> updateText, bool defaultShown) {
            DebugUI_FoldoutGroup foldout = UGUIDebugUI_Helper.InstantiateFoldout(updateText, defaultShown);
            Add(foldout);
            _groups.Push(foldout);

            return this;
        }

        public IDebugUIBuilder IfGroup(Func<bool> condition) {
            DebugUI_IfGroup group = UGUIDebugUI_Helper.InstantiateIfGroup(condition);
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

            DebugUI_ElseGroup group = UGUIDebugUI_Helper.InstantiateElseGroup(ifGroup);
            Add(group);
            _groups.Push(group);

            return this;
        }

        public IDebugUIBuilder SwitchGroup<T>(Func<T> condition) {
            DebugUI_SwitchGroup<T> group = UGUIDebugUI_Helper.InstantiateSwitchGroup(condition);
            Add(group);

            return this;
        }

        public IDebugUIBuilder CaseGroup<T>(Func<T> value) {
            DebugUI_CaseGroup<T> group = UGUIDebugUI_Helper.InstantiateCaseGroup(value);
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