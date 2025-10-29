using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace CookieUtils.Debugging
{
    internal class DebugUIBuilder : IDebugUIBuilder
    {
        private static readonly Dictionary<GameObject, DebugUICanvas> DebugUICanvases = new();
        private readonly GameObject _host;

        internal DebugUIBuilder(GameObject host) => _host = host;

        #region IDebugUIBuilder Members

        public IDebugUIBuilder Label(string text, string id) {
            GetDebugUICanvas(_host).GetPanel(_host).GetLabel(text, id);

            return this;
        }

        public IDebugUIBuilder Foldout(string text, string id) {
            GetDebugUICanvas(_host).GetPanel(_host).Foldout(text, id);

            return this;
        }

        public IDebugUIBuilder EndFoldout() {
            GetDebugUICanvas(_host).GetPanel(_host).EndFoldout();

            return this;
        }

        #endregion

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void StaticInit() {
            CookieDebug.OnExitPlaymode += OnExitPlaymode;
            CookieDebug.OnDebugModeChanged += OnDebugModeChanged;
        }

        private static void OnDebugModeChanged(bool state) {
            GameObject[] canvases = DebugUICanvases.Keys.ToArray();

            for (int i = canvases.Length - 1; i >= 0; i--) {
                GameObject host = canvases[i];
                DebugUICanvas canvas = DebugUICanvases[host];

                if (!host || !canvas) {
                    DebugUICanvases.Remove(host);

                    continue;
                }

                canvas.gameObject.SetActive(state);
            }
        }

        private static void OnExitPlaymode() {
            DebugUICanvases.Clear();
        }

        private static DebugUICanvas GetDebugUICanvas(GameObject host) {
            if (DebugUICanvases.TryGetValue(host, out DebugUICanvas canvas)) return canvas;

            GameObject canvasObject = new($"Debug UI Canvas_{host.gameObject.name}") {
                transform = {
                    localScale = Vector3.one * 0.01f,
                },
            };

            canvas = canvasObject.AddComponent<DebugUICanvas>();

            canvasObject.SetActive(CookieDebug.IsDebugMode);

            DebugUICanvases[host] = canvas;

            return canvas;
        }
    }

    /// <summary>
    ///     Used for creating debug UIs
    /// </summary>
    /// <seealso cref="IDebugDrawer" />
    /// <seealso cref="Label" />
    [PublicAPI]
    public interface IDebugUIBuilder
    {
        /// <summary>
        ///     Draws a label
        /// </summary>
        /// <param name="text">The text to display on the label</param>
        /// <param name="id">The label's id used for internal purposes, do not use the same id multiple times with the same host</param>
        /// <returns>The <see cref="IDebugUIBuilder" /> instance to chain methods</returns>
        public IDebugUIBuilder Label(string text, string id);

        /// <summary>
        ///     Starts a foldout
        /// </summary>
        /// <param name="text">The text displayed next to the foldout</param>
        /// <param name="id">The foldout's id used for internal purposes, do not use the same id multiple times with the same host</param>
        /// <returns>The <see cref="IDebugUIBuilder" /> instance to chain methods</returns>
        public IDebugUIBuilder Foldout(string text, string id);

        /// <summary>
        ///     Ends the current foldout
        /// </summary>
        /// <returns>The <see cref="IDebugUIBuilder" /> instance to chain methods</returns>
        public IDebugUIBuilder EndFoldout();
    }
}