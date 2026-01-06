using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CookieUtils.Debugging
{
    // ReSharper disable once InconsistentNaming
    internal class DebugUI_Button : DebugUI_Element
    {
        [SerializeField]
        private DebugUI_InteractListener listener;

        [SerializeField]
        private TMP_Text label;

        private Func<string> _updateText;
        private Action _onClicked;

        public void OnClicked()
        {
            try
            {
                _onClicked();
            }
            catch (MissingReferenceException)
            {
                OnMissingReference?.Invoke();
            }
        }

        protected override void OnLateUpdate()
        {
            label.text = _updateText();
        }

        internal void Init(Func<string> updateText, Action onClicked)
        {
            _updateText = updateText;
            _onClicked = onClicked;
            listener.onClick.AddListener(OnClicked);
        }
    }
}
