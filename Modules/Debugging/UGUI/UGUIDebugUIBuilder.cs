using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CookieUtils.Debugging
{
    internal class UGUIDebugUIBuilder : IDebugUIBuilder
    {
        private static readonly Dictionary<GameObject, DebugUI_Canvas> DebugUICanvases = new();
        private readonly GameObject _host;

        internal UGUIDebugUIBuilder(GameObject host) => _host = host;
        private DebugUI_Panel Panel => GetDebugUICanvas(_host).GetPanel();

        public IDebugUIBuilder Label(Func<string> updateText) {
            Panel.Label(updateText);

            return this;
        }

        public IDebugUIBuilder FloatField(string text, Func<float> updateValue, Action<float> onValueEdited) {
            Panel.FloatField(text, updateValue, onValueEdited);

            return this;
        }

        public IDebugUIBuilder IntField(string text, Func<int> updateValue, Action<int> onValueEdited) {
            Panel.IntField(text, updateValue, onValueEdited);

            return this;
        }

        public IDebugUIBuilder BoolField(string text, Func<bool> updateValue, Action<bool> onValueEdited) {
            Panel.BoolField(text, updateValue, onValueEdited);

            return this;
        }

        public IDebugUIBuilder StringField(string text, Func<string> updateValue, Action<string> onValueEdited) {
            Panel.StringField(text, updateValue, onValueEdited);

            return this;
        }

        public IDebugUIBuilder Vector2Field(
            string text,
            Func<Vector2> updateValue,
            Action<Vector2> onValueEdited,
            string xLabel = "x",
            string yLabel = "y"
        ) {
            Panel.Vector2Field(text, updateValue, onValueEdited, xLabel, yLabel);

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
            Panel.Vector3Field(text, updateValue, onValueEdited, xLabel, yLabel, zLabel);

            return this;
        }

        public IDebugUIBuilder Button(Func<string> updateText, Action onClicked) {
            Panel.Button(updateText, onClicked);

            return this;
        }

        public IDebugUIBuilder FoldoutGroup(Func<string> updateText, bool defaultShown) {
            Panel.FoldoutGroup(updateText, defaultShown);

            return this;
        }

        public IDebugUIBuilder IfGroup(Func<bool> condition) {
            Panel.IfGroup(condition);

            return this;
        }

        public IDebugUIBuilder ElseGroup() {
            Panel.ElseGroup();

            return this;
        }

        public IDebugUIBuilder SwitchGroup<T>(Func<T> condition) {
            Panel.SwitchGroup(condition);

            return this;
        }

        public IDebugUIBuilder CaseGroup<T>(Func<T> value) {
            Panel.CaseGroup(value);

            return this;
        }

        public IDebugUIBuilder EndGroup() {
            Panel.EndGroup();

            return this;
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void StaticInit() {
            CookieDebug.OnExitPlaymode += OnExitPlaymode;
            CookieDebug.OnDebugModeChanged += OnDebugModeChanged;
        }

        private static void OnDebugModeChanged(bool state) {
            GameObject[] canvases = DebugUICanvases.Keys.ToArray();

            for (int i = canvases.Length - 1; i >= 0; i--) {
                GameObject host = canvases[i];
                DebugUI_Canvas canvas = DebugUICanvases[host];

                if (!host || !canvas) DebugUICanvases.Remove(host);
            }
        }

        private static void OnExitPlaymode() {
            DebugUICanvases.Clear();
        }

        private static DebugUI_Canvas GetDebugUICanvas(GameObject host) {
            if (DebugUICanvases.TryGetValue(host, out DebugUI_Canvas canvas) && canvas) return canvas;

            GameObject canvasObject = new($"Debug UI Canvas_{host.gameObject.name}") {
                transform = {
                    localScale = Vector3.one * 0.01f,
                },
            };

            canvas = canvasObject.AddComponent<DebugUI_Canvas>();
            canvas.Init(host);

            canvasObject.SetActive(CookieDebug.IsDebugMode);

            DebugUICanvases[host] = canvas;

            return canvas;
        }
    }
}