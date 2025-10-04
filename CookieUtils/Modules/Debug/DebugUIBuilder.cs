using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace CookieUtils
{
    internal class DebugUIBuilder : IDebugUIBuilder
    {
        private static readonly Dictionary<GameObject, Canvas> DebugUICanvases = new();
        private static readonly Dictionary<GameObject, DebugUIPanel> Panels = new();
        private static readonly DebugUIPanel PanelPrefab = Resources.Load<DebugUIPanel>("DebugUI/Prefabs/Panel");
        private static readonly Vector3 DefaultOffset = Vector3.up;
        private readonly GameObject _host;

        internal DebugUIBuilder(GameObject host)
        {
            _host = host;
        }
        
        public IDebugUIBuilder Label(string text, string id)
        {
            GetPanel(_host).GetLabel(text, id);
            return this;
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void StaticInit()
        {
            CookieDebug.OnExitPlaymode += OnExitPlaymode;
            CookieDebug.OnDebugModeChanged += OnDebugModeChanged;
        }

        private static void OnDebugModeChanged(bool state)
        {
            foreach (var debugUICanvas in DebugUICanvases.Values) {
                debugUICanvas.gameObject.SetActive(state);
            }
        }

        private static void OnExitPlaymode()
        {
            DebugUICanvases.Clear();
            Panels.Clear();
        }

        private static DebugUIPanel GetPanel(GameObject host)
        {
            if (Panels.TryGetValue(host, out var panel)) return panel;

            var canvas = GetDebugUICanvas(host);
            panel = Object.Instantiate(PanelPrefab, canvas.transform);
            
            Panels[host] = panel;
            
            return panel;
        }

        private static Canvas GetDebugUICanvas(GameObject host)
        {
            if (DebugUICanvases.TryGetValue(host, out var canvas)) return canvas;

            var canvasObject = new GameObject("Debug UI Canvas");
            canvasObject.transform.SetParent(host.transform, false);
            canvasObject.transform.localScale = Vector3.one * 0.01f;

            // shouldn't be a big hit to performance because once it's called, the canvas is stored in DebugUICanvases
            var renderer = host.GetComponentInChildren<Renderer>();
            if (renderer) {
                canvasObject.transform.localPosition = Vector3.up * (renderer.localBounds.max.y + 0.25f);
            } else canvasObject.transform.position = DefaultOffset;
            
            canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;

            canvasObject.SetActive(CookieDebug.IsDebugMode);
            
            DebugUICanvases[host] = canvas;
            
            return canvas;
        }
    }
    
    /// <summary>
    /// Used for creating debug UIs
    /// </summary>
    /// <seealso cref="IDebugDrawer"/>
    /// <seealso cref="Label"/>
    [PublicAPI]
    public interface IDebugUIBuilder
    {
        /// <summary>
        /// Draws a label
        /// </summary>
        /// <param name="text">The text to display on the label</param>
        /// <param name="id">The label's id used for internal purposes, do not use the same id with the same host</param>
        /// <returns>The IDebugUIBuilder instance to chain methods</returns>
        public IDebugUIBuilder Label(string text, string id) => this;
    }
}