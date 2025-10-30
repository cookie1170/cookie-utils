using System;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

namespace CookieUtils.Debugging
{
    // ReSharper disable once InconsistentNaming
    internal class DebugUI_Vector3Field : DebugUI_Field
    {
        [SerializeField] private TMP_Text xLabel;
        [SerializeField] private TMP_Text yLabel;
        [SerializeField] private TMP_Text zLabel;
        [SerializeField] private TMP_InputField xInput;
        [SerializeField] private TMP_InputField yInput;
        [SerializeField] private TMP_InputField zInput;

        private Action<Vector3> _onValueEdited;
        private Func<Vector3> _updateValue;

        private void Awake() {
            xInput.onSubmit.AddListener(OnSubmit);
            yInput.onSubmit.AddListener(OnSubmit);
            zInput.onSubmit.AddListener(OnSubmit);
        }

        protected override void OnLateUpdate() {
            if (xInput.isFocused || yInput.isFocused || zInput.isFocused) return;

            Vector3 value = _updateValue();

            xInput.text = value.x.ToString("0.0");
            yInput.text = value.y.ToString("0.0");
            zInput.text = value.z.ToString("0.0");
        }

        protected override void OnDestroyed() {
            xInput.onSubmit.RemoveAllListeners();
            yInput.onSubmit.RemoveAllListeners();
            zInput.onSubmit.RemoveAllListeners();
        }

        private void OnSubmit(string newValue) {
            try {
                _onValueEdited?.Invoke(
                    new Vector3(float.Parse(xInput.text), float.Parse(yInput.text), float.Parse(zInput.text))
                );
            }
            catch (MissingReferenceException) {
                OnMissingReference?.Invoke();
            }
        }

        internal void Init(
            string text,
            Func<Vector3> updateValue,
            [CanBeNull] Action<Vector3> onValueEdited,
            string xText,
            string yText,
            string zText
        ) {
            label.text = text;
            xLabel.text = xText;
            yLabel.text = yText;
            zLabel.text = zText;
            _updateValue = updateValue;

            if (onValueEdited == null) {
                xInput.interactable = false;
                yInput.interactable = false;
                zInput.interactable = false;

                return;
            }

            _onValueEdited = onValueEdited;
        }
    }
}