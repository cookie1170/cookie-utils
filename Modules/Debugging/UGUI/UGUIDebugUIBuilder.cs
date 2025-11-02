using System;
using UnityEngine;

namespace CookieUtils.Debugging
{
    internal class UGUIDebugUIBuilder : IDebugUIBuilder
    {
        private readonly DebugUI_Panel _panel;

        internal UGUIDebugUIBuilder(DebugUI_Panel panel) => _panel = panel;

        public IDebugUIBuilder Label(Func<string> updateText) {
            _panel.Label(updateText);

            return this;
        }

        public IDebugUIBuilder FloatField(string text, Func<float> updateValue, Action<float> onValueEdited) {
            _panel.FloatField(text, updateValue, onValueEdited);

            return this;
        }

        public IDebugUIBuilder IntField(string text, Func<int> updateValue, Action<int> onValueEdited) {
            _panel.IntField(text, updateValue, onValueEdited);

            return this;
        }

        public IDebugUIBuilder BoolField(string text, Func<bool> updateValue, Action<bool> onValueEdited) {
            _panel.BoolField(text, updateValue, onValueEdited);

            return this;
        }

        public IDebugUIBuilder StringField(string text, Func<string> updateValue, Action<string> onValueEdited) {
            _panel.StringField(text, updateValue, onValueEdited);

            return this;
        }

        public IDebugUIBuilder Vector2Field(
            string text,
            Func<Vector2> updateValue,
            Action<Vector2> onValueEdited,
            string xLabel = "x",
            string yLabel = "y"
        ) {
            _panel.Vector2Field(text, updateValue, onValueEdited, xLabel, yLabel);

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
            _panel.Vector3Field(text, updateValue, onValueEdited, xLabel, yLabel, zLabel);

            return this;
        }

        public IDebugUIBuilder Button(Func<string> updateText, Action onClicked) {
            _panel.Button(updateText, onClicked);

            return this;
        }

        public IDebugUIBuilder FoldoutGroup(Func<string> updateText, bool defaultShown) {
            _panel.FoldoutGroup(updateText, defaultShown);

            return this;
        }

        public IDebugUIBuilder IfGroup(Func<bool> condition) {
            _panel.IfGroup(condition);

            return this;
        }

        public IDebugUIBuilder ElseGroup() {
            _panel.ElseGroup();

            return this;
        }

        public IDebugUIBuilder SwitchGroup<T>(Func<T> condition) {
            _panel.SwitchGroup(condition);

            return this;
        }

        public IDebugUIBuilder CaseGroup<T>(Func<T> value) {
            _panel.CaseGroup(value);

            return this;
        }

        public IDebugUIBuilder EndGroup() {
            _panel.EndGroup();

            return this;
        }
    }
}