using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace CookieUtils.Debugging
{
    // ReSharper disable once InconsistentNaming
    internal class DebugUI_BoolField : DebugUI_Field
    {
        [SerializeField] private Toggle toggle;

        private Action<bool> _onValueEdited;
        private Func<bool> _updateValue;

        private void Awake() {
            toggle.onValueChanged.AddListener(OnValueChanged);
        }

        protected override void OnLateUpdate() {
            toggle.isOn = _updateValue();
        }

        protected override void OnDestroyed() {
            toggle.onValueChanged.RemoveAllListeners();
        }

        private void OnValueChanged(bool newValue) {
            try {
                _onValueEdited?.Invoke(newValue);
            }
            catch (MissingReferenceException) {
                OnMissingReference?.Invoke();
            }
        }

        internal void Init(string text, Func<bool> updateValue, [CanBeNull] Action<bool> onValueEdited) {
            label.text = text;
            _updateValue = updateValue;

            if (onValueEdited == null) {
                toggle.interactable = false;

                return;
            }

            _onValueEdited = onValueEdited;
        }
    }
}