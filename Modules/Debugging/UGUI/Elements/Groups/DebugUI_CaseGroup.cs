using System;

namespace CookieUtils.Debugging
{
    // ReSharper disable once InconsistentNaming
    internal class DebugUI_CaseGroup<T> : DebugUI_Group
    {
        private Func<T> _value;

        internal void Init(Func<T> value) {
            _value = value;
        }

        internal void OnEvaluated(T result) {
            Content.gameObject.SetActive(result.Equals(_value()));
        }
    }
}