using TMPro;

namespace CookieUtils.Debugging
{
    // ReSharper disable once InconsistentNaming
    internal class DebugUI_FloatField : DebugUI_TextField<float>
    {
        protected override TMP_InputField.ContentType ContentType => TMP_InputField.ContentType.DecimalNumber;
        protected override float Parse(string newValue) => float.Parse(newValue);
        protected override string ToString(float value) => value.ToString("0.00");
    }
}