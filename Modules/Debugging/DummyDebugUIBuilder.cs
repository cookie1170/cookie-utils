using System;
using UnityEngine;

namespace CookieUtils.Debugging
{
    internal class DummyDebugUIBuilder : IDebugUI_If
    {
        public void Label(Func<string> updateText) { }

        public void FloatField(string text, Func<float> updateValue) { }

        public void FloatField(
            string text,
            Func<float> updateValue,
            Action<float> onValueEdited
        ) { }

        public void IntField(string text, Func<int> updateValue) { }

        public void IntField(string text, Func<int> updateValue, Action<int> onValueEdited) { }

        public void BoolField(string text, Func<bool> updateValue) { }

        public void BoolField(string text, Func<bool> updateValue, Action<bool> onValueEdited) { }

        public void StringField(string text, Func<string> updateValue) { }

        public void StringField(
            string text,
            Func<string> updateValue,
            Action<string> onValueEdited
        ) { }

        public void Vector2Field(
            string text,
            Func<Vector2> updateValue,
            Action<Vector2> onValueEdited,
            string xLabel = "x",
            string yLabel = "y"
        ) { }

        public void Vector3Field(
            string text,
            Func<Vector3> updateValue,
            Action<Vector3> onValueEdited,
            string xLabel = "x",
            string yLabel = "y",
            string zLabel = "z"
        ) { }

        public void Button(Func<string> updateText, Action onClicked) { }

        public IDebugUI_Group FoldoutGroup(Func<string> updateText, bool defaultShown = true) =>
            this;

        public IDebugUI_If IfGroup(Func<bool> condition) => this;

        public IDebugUI_Group ElseGroup() => this;
    }
}
