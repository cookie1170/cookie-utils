using System;
using JetBrains.Annotations;

namespace CookieUtils.Debugging
{
    // ReSharper disable once InconsistentNaming
    internal class DebugUI_BoolField : DebugUI_Field
    {
        internal void Init(string text, Func<bool> updateValue, [CanBeNull] Action<bool> onValueEdited) {
            label.text = text;
        }
    }
}