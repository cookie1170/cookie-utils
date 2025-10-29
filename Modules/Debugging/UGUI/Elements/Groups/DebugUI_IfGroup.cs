using System;

namespace CookieUtils.Debugging
{
    // ReSharper disable once InconsistentNaming
    internal class DebugUI_IfGroup : DebugUI_Group
    {
        private Func<bool> _condition;

        private void LateUpdate() {
            bool result = _condition();
            Content.gameObject.SetActive(result);
            OnConditionEvaluated?.Invoke(result);
        }

        internal event Action<bool> OnConditionEvaluated;

        internal void Init(Func<bool> condition) {
            _condition = condition;
        }
    }
}