using System;
using UnityEngine;

namespace CookieUtils.Debugging
{
    // ReSharper disable once InconsistentNaming
    internal abstract class DebugUI_Element : MonoBehaviour
    {
        internal Action OnMissingReference;

        private void LateUpdate() {
            try {
                OnLateUpdate();
            }
            catch (MissingReferenceException) {
                OnMissingReference?.Invoke();
            }
        }

        private void OnDestroy() {
            OnMissingReference = null;
            OnDestroyed();
        }

        protected virtual void OnDestroyed() { }

        protected virtual void OnLateUpdate() { }
    }
}