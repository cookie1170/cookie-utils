using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CookieUtils.Debugging
{
    internal class UGUIDebugUIBuilderProvider : IDebugUIBuilderProvider
    {
        private static readonly Dictionary<GameObject, DebugUI_Canvas> DebugUICanvases = new();

        public IDebugUIBuilder GetFor(GameObject host) {
            if (host) return new UGUIDebugUIBuilder(GetDebugUICanvas(host).GetPanel());

            return new DummyDebugUIBuilder();
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