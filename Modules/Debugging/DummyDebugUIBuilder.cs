using System;
using UnityEngine;

namespace CookieUtils.Debugging
{
    internal class DummyDebugUIBuilder : IDebugUIBuilder
    {
        public IDebugUIBuilder Label(Func<string> updateText) => this;

        public IDebugUIBuilder FloatField(string text, Func<float> updateValue) => this;

        public IDebugUIBuilder FloatField(string text, Func<float> updateValue, Action<float> onValueEdited) => this;

        public IDebugUIBuilder IntField(string text, Func<int> updateValue) => this;

        public IDebugUIBuilder IntField(string text, Func<int> updateValue, Action<int> onValueEdited) => this;

        public IDebugUIBuilder BoolField(string text, Func<bool> updateValue) => this;

        public IDebugUIBuilder BoolField(string text, Func<bool> updateValue, Action<bool> onValueEdited) => this;

        public IDebugUIBuilder StringField(string text, Func<string> updateValue) => this;

        public IDebugUIBuilder StringField(string text, Func<string> updateValue, Action<string> onValueEdited) => this;

        public IDebugUIBuilder Vector2Field(string text, Func<Vector2> updateValue, string xLabel = "x", string yLabel = "y") => this;

        public IDebugUIBuilder Vector2Field(
            string text,
            Func<Vector2> updateValue,
            Action<Vector2> onValueEdited,
            string xLabel = "x",
            string yLabel = "y"
        ) =>
            this;

        public IDebugUIBuilder Vector3Field(
            string text,
            Func<Vector3> updateValue,
            string xLabel = "x",
            string yLabel = "y",
            string zLabel = "z"
        ) =>
            this;

        public IDebugUIBuilder Vector3Field(
            string text,
            Func<Vector3> updateValue,
            Action<Vector3> onValueEdited,
            string xLabel = "x",
            string yLabel = "y",
            string zLabel = "z"
        ) =>
            this;

        public IDebugUIBuilder Button(Func<string> updateText, Action onClicked) => this;

        public IDebugUIBuilder FoldoutGroup(Func<string> updateText, bool defaultShown = true) => this;

        public IDebugUIBuilder IfGroup(Func<bool> condition) => this;

        public IDebugUIBuilder ElseGroup() => this;

        public IDebugUIBuilder SwitchGroup<T>(Func<T> condition) => this;

        public IDebugUIBuilder CaseGroup<T>(Func<T> value) => this;

        public IDebugUIBuilder EndGroup() => this;
    }
}