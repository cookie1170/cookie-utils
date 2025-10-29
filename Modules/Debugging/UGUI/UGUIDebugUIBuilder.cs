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