using TMPro;

namespace CookieUtils.Debugging
{
    // ReSharper disable once InconsistentNaming
    internal class DebugUI_IntField : DebugUI_TextField<int>
    {
        protected override TMP_InputField.ContentType ContentType => TMP_InputField.ContentType.IntegerNumber;
        protected override int Parse(string newValue) => int.Parse(newValue);
        protected override string ToString(int value) => value.ToString();
    }
}