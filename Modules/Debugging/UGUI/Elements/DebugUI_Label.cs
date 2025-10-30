using System;
using TMPro;

namespace CookieUtils.Debugging
{
    // ReSharper disable once InconsistentNaming
    internal class DebugUI_Label : DebugUI_Element
    {
        private TMP_Text _text;
        private Func<string> _updateText;

        private TMP_Text Text {
            get {
                if (!_text) _text = GetComponent<TMP_Text>();

                return _text;
            }
        }

        protected override void OnLateUpdate() {
            Text.text = _updateText();
        }

        internal void Init(Func<string> updateText) {
            _updateText = updateText;
        }
    }
}