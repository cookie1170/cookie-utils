using System;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

namespace CookieUtils.Debugging
{
    // ReSharper disable once InconsistentNaming
    internal abstract class DebugUI_TextField<T> : DebugUI_Field
    {
        [SerializeField]
        protected TMP_InputField inputField;
        private Action<T> _onValueEdited;
        private Func<T> _updateValue;
        protected abstract TMP_InputField.ContentType ContentType { get; }

        private void Awake()
        {
            inputField.onSubmit.AddListener(OnSubmit);
            inputField.contentType = ContentType;
        }

        protected override void OnLateUpdate()
        {
            if (inputField.isFocused)
                return;

            inputField.text = ToString(_updateValue());
        }

        protected override void OnDestroyed()
        {
            inputField.onSubmit.RemoveAllListeners();
        }

        private void OnSubmit(string newValue)
        {
            try
            {
                _onValueEdited?.Invoke(Parse(newValue));
            }
            catch (MissingReferenceException)
            {
                OnMissingReference?.Invoke();
            }
        }

        protected abstract T Parse(string newValue);
        protected abstract string ToString(T value);

        internal void Init(string text, Func<T> updateValue, [CanBeNull] Action<T> onValueEdited)
        {
            label.text = text;
            _updateValue = updateValue;

            if (onValueEdited == null)
            {
                inputField.interactable = false;

                return;
            }

            _onValueEdited = onValueEdited;
        }
    }
}
