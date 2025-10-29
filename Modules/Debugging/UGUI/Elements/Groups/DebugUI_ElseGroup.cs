namespace CookieUtils.Debugging
{
    // ReSharper disable once InconsistentNaming
    internal class DebugUI_ElseGroup : DebugUI_Group
    {
        private DebugUI_IfGroup _ifGroup;

        private void OnDestroy() => _ifGroup.OnConditionEvaluated -= OnConditionEvaluated;

        internal void Init(DebugUI_IfGroup ifGroup) {
            _ifGroup = ifGroup;
            _ifGroup.OnConditionEvaluated += OnConditionEvaluated;
        }

        private void OnConditionEvaluated(bool result) {
            Content.gameObject.SetActive(!result);
        }
    }
}