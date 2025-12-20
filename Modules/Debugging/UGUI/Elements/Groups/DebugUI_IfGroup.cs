using System;
using UnityEngine;

namespace CookieUtils.Debugging
{
    // ReSharper disable once InconsistentNaming
    internal class DebugUI_IfGroup : DebugUI_Group
    {
        private Func<bool> _condition;

        protected override void OnLateUpdate()
        {
            bool result = _condition();
            Content.gameObject.SetActive(result);
            OnConditionEvaluated?.Invoke(result);
        }

        internal override void AddChild(GameObject child)
        {
            if (child.TryGetComponent(out DebugUI_ElseGroup elseGroup))
            {
                elseGroup.Init(this);
                var parentGroup = transform.parent.parent.GetComponent<DebugUI_Group>();
                if (parentGroup != null)
                    parentGroup.AddChild(elseGroup.gameObject);

                return;
            }

            base.AddChild(child);
        }

        internal event Action<bool> OnConditionEvaluated;

        internal void Init(Func<bool> condition)
        {
            _condition = condition;
        }
    }
}
