using System;
using TMPro;
using UnityEngine;

namespace CookieUtils.Debugging
{
    // ReSharper disable once InconsistentNaming
    internal class DebugUI_Label : MonoBehaviour
    {
        private TMP_Text _text;
        private Func<string> _updateText;

        private TMP_Text Text {
            get {
                if (!_text) _text = GetComponent<TMP_Text>();

                return _text;
            }
        }

        private void LateUpdate() {
            Text.text = _updateText();
        }

        internal void Init(Func<string> updateText) {
            _updateText = updateText;
        }
    }
}