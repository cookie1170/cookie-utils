using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace CookieUtils.Debugging
{
    // ReSharper disable once InconsistentNaming
    internal class DebugUI_BoolField : DebugUI_Field
    {
        [SerializeField]
        private DebugUI_InteractListener listener;

        [SerializeField]
        private Image checkmark;

        private Action<bool> _onValueEdited;
        private Func<bool> _updateValue;
        private bool _value;

        private void Awake()
        {
            listener.onClick.AddListener(OnClicked);
        }

        protected override void OnLateUpdate()
        {
            checkmark.enabled = _value;
            _value = _updateValue();
        }

        internal void Init(
            string text,
            Func<bool> updateValue,
            [CanBeNull] Action<bool> onValueEdited
        )
        {
            label.text = text;
            _updateValue = updateValue;
            _value = updateValue();

            if (onValueEdited == null)
            {
                listener.interactable = false;

                return;
            }

            _onValueEdited = onValueEdited;
        }

        public void OnClicked()
        {
            _value = !_value;

            try
            {
                _onValueEdited?.Invoke(_value);
            }
            catch (MissingReferenceException)
            {
                OnMissingReference?.Invoke();
            }
        }
    }
}
