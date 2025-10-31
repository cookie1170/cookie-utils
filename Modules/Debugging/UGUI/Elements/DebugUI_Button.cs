using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CookieUtils.Debugging
{
    // ReSharper disable once InconsistentNaming
    internal class DebugUI_Button : DebugUI_Element
    {
        [SerializeField] private Button button;
        [SerializeField] private TMP_Text label;

        private Func<string> _updateText;

        protected override void OnDestroyed() {
            button.onClick.RemoveAllListeners();
        }

        protected override void OnLateUpdate() {
            label.text = _updateText();
        }

        internal void Init(Func<string> updateText, Action onClicked) {
            _updateText = updateText;
            button.onClick.AddListener(() => {
                    try {
                        onClicked();
                    }
                    catch (MissingReferenceException) {
                        OnMissingReference?.Invoke();
                    }
                }
            );
        }
    }
}