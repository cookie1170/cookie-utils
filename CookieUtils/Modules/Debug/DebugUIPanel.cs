using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace CookieUtils
{
    internal class DebugUIPanel : MonoBehaviour
    {
        private TMP_Text _labelPrefab;
        private readonly Dictionary<string, TMP_Text> _labels = new();

        private void Awake()
        {
            _labelPrefab = Resources.Load<TMP_Text>("DebugUI/Prefabs/Text");
        }
        
        public void GetLabel(string text, string id)
        {
            if (!_labels.TryGetValue(id, out var label)) {
                if (!_labelPrefab) _labelPrefab = Resources.Load<TMP_Text>("DebugUI/Prefabs/Text");
                
                label = Instantiate(_labelPrefab, transform);
                label.name += $" {id}";
                _labels[id] = label;
            }
            
            label.text = text;
        }
    }
}
