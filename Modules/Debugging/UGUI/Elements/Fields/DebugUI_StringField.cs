using TMPro;

namespace CookieUtils.Debugging
{
    // ReSharper disable once InconsistentNaming
    internal class DebugUI_StringField : DebugUI_TextField<string>
    {
        protected override TMP_InputField.ContentType ContentType =>
            TMP_InputField.ContentType.Standard;

        protected override string Parse(string newValue) => newValue;

        protected override string ToString(string value) => value;
    }
}
