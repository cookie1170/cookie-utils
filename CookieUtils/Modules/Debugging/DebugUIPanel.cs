using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CookieUtils.Debugging
{
    internal class DebugUIPanel : MonoBehaviour
    {
        private readonly List<(DebugUIFoldout foldout, int childIndex)> _activeFoldouts = new();
        private readonly Dictionary<string, (DebugUIFoldout obj, float lastUsed)> _foldouts = new();

        private readonly Dictionary<string, (TMP_Text obj, float lastUsed)> _labels = new();
        private DebugUIFoldout _foldoutPrefab;
        private Image _imageCached;

        private TMP_Text _labelPrefab;
        private float _refreshTime;
        private int _rootIndex = 0;

        public Image Image {
            get {
                if (_imageCached) return _imageCached;

                _imageCached = GetComponent<Image>();
                return _imageCached;
            }
        }

        private int Index {
            get => _activeFoldouts.Count > 0 ? _activeFoldouts.Last().childIndex : _rootIndex;
            set {
                if (_activeFoldouts.Count > 0) {
                    (DebugUIFoldout foldout, int childIndex) foldout = _activeFoldouts[^1];
                    foldout.childIndex = value;
                    _activeFoldouts[^1] = foldout;
                } else {
                    _rootIndex = value;
                }
            }
        }

        private Transform Parent => _activeFoldouts.Count > 0 ? _activeFoldouts.Last().foldout.content : transform;

        private void Awake()
        {
            _labelPrefab = Resources.Load<TMP_Text>("DebugUI/Prefabs/Text");
            _foldoutPrefab = Resources.Load<DebugUIFoldout>("DebugUI/Prefabs/Foldout");
            _refreshTime = CookieDebug.DebuggingSettings.refreshTime;
        }

        private void Update()
        {
            if (!CookieDebug.IsDebugMode) return;

            Index = 0;

            string[] keysLabel = _labels.Keys.ToArray();

            for (int i = keysLabel.Length - 1; i >= 0; i--) {
                string key = keysLabel[i];
                (TMP_Text obj, float lastUsed) value = _labels[key];
                value.lastUsed += Time.unscaledDeltaTime;
                _labels[key] = value;
            }

            string[] keysFoldout = _foldouts.Keys.ToArray();

            for (int i = keysFoldout.Length - 1; i >= 0; i--) {
                string key = keysFoldout[i];
                (DebugUIFoldout obj, float lastUsed) value = _foldouts[key];
                value.lastUsed += Time.unscaledDeltaTime;
                _foldouts[key] = value;
            }
        }

        private void LateUpdate()
        {
            if (!CookieDebug.IsDebugMode) return;

            string[] keysLabel = _labels.Keys.ToArray();

            for (int i = keysLabel.Length - 1; i >= 0; i--) {
                string key = keysLabel[i];
                (TMP_Text obj, float lastUsed) value = _labels[key];

                if (value.lastUsed > _refreshTime) {
                    _labels.Remove(key);
                    Destroy(value.obj.gameObject);
                }
            }

            string[] keysFoldout = _foldouts.Keys.ToArray();

            for (int i = keysFoldout.Length - 1; i >= 0; i--) {
                string key = keysFoldout[i];
                (DebugUIFoldout obj, float lastUsed) value = _foldouts[key];

                if (value.lastUsed > _refreshTime) {
                    _foldouts.Remove(key);
                    Destroy(value.obj.gameObject);
                }
            }
        }

        public void GetLabel(string text, string id)
        {
            if (!_labels.TryGetValue(id, out (TMP_Text obj, float lastUsed) label) || !label.obj) {
                if (!_labelPrefab) _labelPrefab = Resources.Load<TMP_Text>("DebugUI/Prefabs/Text");

                label.obj = Instantiate(_labelPrefab, Parent);
                label.obj.name += $" {id}";
            }

            label.obj.transform.SetParent(Parent, false);
            label.obj.transform.SetSiblingIndex(Index++);
            label.obj.text = text;
            label.lastUsed = 0;
            _labels[id] = label;
        }

        public void Foldout(string text, string id)
        {
            _activeFoldouts.Add((GetFoldout(text, id), Index));
        }

        private DebugUIFoldout GetFoldout(string text, string id)
        {
            if (!_foldouts.TryGetValue(id, out (DebugUIFoldout obj, float lastUsed) foldout) || !foldout.obj) {
                if (!_foldoutPrefab) _foldoutPrefab = Resources.Load<DebugUIFoldout>("DebugUI/Prefabs/Foldout");

                foldout.obj = Instantiate(_foldoutPrefab, Parent);
                foldout.obj.name += $" {id}";
            }

            foldout.obj.transform.SetParent(Parent, false);
            foldout.obj.transform.SetSiblingIndex(Index++);
            foldout.obj.SetText(text);
            foldout.lastUsed = 0;
            _foldouts[id] = foldout;

            return foldout.obj;
        }

        public void EndFoldout()
        {
            if (_activeFoldouts.Count > 0) _activeFoldouts.RemoveAt(_activeFoldouts.Count - 1);
        }
    }
}