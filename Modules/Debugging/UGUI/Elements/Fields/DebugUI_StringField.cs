using System;
using JetBrains.Annotations;

namespace CookieUtils.Debugging
{
    // ReSharper disable once InconsistentNaming
    internal class DebugUI_StringField : DebugUI_TextField
    {
        internal void Init(string text, Func<string> updateValue, [CanBeNull] Action<string> onValueEdited) {
            label.text = text;
        }
    }
}