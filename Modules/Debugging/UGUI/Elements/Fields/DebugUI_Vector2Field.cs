using System;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

namespace CookieUtils.Debugging
{
    // ReSharper disable once InconsistentNaming
    internal class DebugUI_Vector2Field : DebugUI_Field
    {
        [SerializeField]
        private TMP_Text xLabel;

        [SerializeField]
        private TMP_Text yLabel;

        [SerializeField]
        private TMP_InputField xInput;

        [SerializeField]
        private TMP_InputField yInput;

        private Action<Vector2> _onValueEdited;
        private Func<Vector2> _updateValue;

        private void Awake()
        {
            xInput.onSubmit.AddListener(OnSubmit);
            yInput.onSubmit.AddListener(OnSubmit);
        }

        protected override void OnLateUpdate()
        {
            if (xInput.isFocused || yInput.isFocused)
                return;

            Vector2 value = _updateValue();

            xInput.text = value.x.ToString("0.0");
            yInput.text = value.y.ToString("0.0");
        }

        protected override void OnDestroyed()
        {
            xInput.onSubmit.RemoveAllListeners();
            yInput.onSubmit.RemoveAllListeners();
        }

        private void OnSubmit(string newValue)
        {
            try
            {
                _onValueEdited?.Invoke(
                    new Vector2(float.Parse(xInput.text), float.Parse(yInput.text))
                );
            }
            catch (MissingReferenceException)
            {
                OnMissingReference?.Invoke();
            }
        }

        internal void Init(
            string text,
            Func<Vector2> updateValue,
            [CanBeNull] Action<Vector2> onValueEdited,
            string xText,
            string yText
        )
        {
            label.text = text;
            xLabel.text = xText;
            yLabel.text = yText;
            _updateValue = updateValue;

            if (onValueEdited == null)
            {
                xInput.interactable = false;
                yInput.interactable = false;

                return;
            }

            _onValueEdited = onValueEdited;
        }
    }
}
