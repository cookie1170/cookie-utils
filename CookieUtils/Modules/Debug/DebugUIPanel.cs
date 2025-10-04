using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace CookieUtils.Debugging
{
    internal class DebugUIPanel : MonoBehaviour
    {
        private TMP_Text _labelPrefab;
        private DebugUIFoldout _foldoutPrefab;

        private int _rootIndex = 0;
        
        private int Index {
            get => _activeFoldouts.Count > 0 ? _activeFoldouts.Last().childIndex : _rootIndex;
            set {
                if (_activeFoldouts.Count > 0) {
                    var foldout = _activeFoldouts[^1];
                    foldout.childIndex = value;
                    _activeFoldouts[^1] = foldout;
                } else _rootIndex = value;
            }
        }

        private Transform Parent => _activeFoldouts.Count > 0 ? _activeFoldouts.Last().foldout.content : transform;

        private readonly Dictionary<string, TMP_Text> _labels = new();
        private readonly Dictionary<string, DebugUIFoldout> _foldouts = new();
        private readonly List<(DebugUIFoldout foldout, int childIndex)> _activeFoldouts = new();
        private readonly Dictionary<GameObject, float> _wasUsed = new();
        
        public void GetLabel(string text, string id)
        {
            if (!_labels.TryGetValue(id, out var label) || !label) {
                if (!_labelPrefab) _labelPrefab = Resources.Load<TMP_Text>("DebugUI/Prefabs/Text");
                
                label = Instantiate(_labelPrefab, Parent);
                label.name += $" {id}";
                _labels[id] = label;
            }

            _wasUsed[label.gameObject] = 0;
            label.transform.SetParent(Parent, false);
            label.transform.SetSiblingIndex(Index++);
            label.text = text;
        }

        public void Foldout(string text, string id)
        {
            _activeFoldouts.Add((GetFoldout(text, id), Index));
        }

        private DebugUIFoldout GetFoldout(string text, string id)
        {
            if (!_foldouts.TryGetValue(id, out var foldout) || !foldout) {
                if (!_foldoutPrefab) _foldoutPrefab = Resources.Load<DebugUIFoldout>("DebugUI/Prefabs/Foldout");
                
                foldout = Instantiate(_foldoutPrefab, Parent);
                foldout.name += $" {id}";
                _foldouts[id] = foldout;
            }

            _wasUsed[foldout.gameObject] = 0;
            foldout.transform.SetParent(Parent, false);
            foldout.transform.SetSiblingIndex(Index++);
            foldout.SetText(text);

            return foldout;
        }
        
        public void EndFoldout()
        {
            if (_activeFoldouts.Count > 0) _activeFoldouts.RemoveAt(_activeFoldouts.Count - 1);
        }
                
        private void Awake()
        {
            _labelPrefab = Resources.Load<TMP_Text>("DebugUI/Prefabs/Text");
            _foldoutPrefab = Resources.Load<DebugUIFoldout>("DebugUI/Prefabs/Foldout");
        }

        private void Update()
        {
            if (!CookieDebug.IsDebugMode) return;

            Index = 0;
            var keys = _wasUsed.Keys.ToArray();
            _activeFoldouts.Clear();
            
            for (int i = keys.Length - 1; i >= 0; i--) {
                var obj = keys[i];
                _wasUsed[obj] += Time.deltaTime;
            }
        }

        private void LateUpdate()
        {
            if (!CookieDebug.IsDebugMode) return;
            
            var keys = _wasUsed.Keys.ToArray();

            for (int i = keys.Length - 1; i >= 0; i--) {
                var obj = keys[i];

                if (_wasUsed[obj] > 0.15f) {
                    _wasUsed.Remove(obj);
                    Destroy(obj);
                }
            }
            
            string[] destroyedLabels = _labels.Where(o => !o.Value).Select(k => k.Key).ToArray();
            for (int i = destroyedLabels.Length - 1; i >= 0; i--) {
                string key = destroyedLabels[i];
                _labels.Remove(key);
            }
            
            string[] destroyedFoldouts = _foldouts.Where(o => !o.Value).Select(k => k.Key).ToArray();
            for (int i = destroyedFoldouts.Length - 1; i >= 0; i--) {
                string key = destroyedFoldouts[i];
                _foldouts.Remove(key);
            }
        }
    }
}
