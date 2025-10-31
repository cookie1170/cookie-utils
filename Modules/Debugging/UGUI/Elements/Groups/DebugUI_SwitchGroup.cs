using System;
using UnityEngine;

namespace CookieUtils.Debugging
{
    // ReSharper disable once InconsistentNaming
    internal class DebugUI_SwitchGroup<T> : DebugUI_Group
    {
        private Func<T> _condition;
        private Action<T> _onEvaluated;

        protected override void OnLateUpdate() {
            T result = _condition();

            _onEvaluated?.Invoke(result);
        }

        protected override void OnDestroyed() {
            _onEvaluated = null;
        }

        internal override void AddChild(GameObject child) {
            if (!child.TryGetComponent(out DebugUI_CaseGroup<T> caseGroup))
                throw new ArgumentException("[CookieUtils.Debugging] Children of a SwitchGroup must be case groups!");

            _onEvaluated += caseGroup.OnEvaluated;

            child.transform.SetParent(Content, false);
        }

        internal void Init(Func<T> condition) {
            _condition = condition;
        }
    }
}